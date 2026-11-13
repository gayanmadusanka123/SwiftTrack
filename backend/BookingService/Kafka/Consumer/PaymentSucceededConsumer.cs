using System;
using System.Text.Json;
using BookingService.Kafka.Event;
using BookingService.Model;
using BookingService.Model.Entity;
using BookingService.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Kafka.Consumer;

public record PaymentSucceededIntegrationEvent(Guid EventId, Guid BookingId, Guid PaymentId, decimal AmountPaid);
public class PaymentSucceededConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PaymentSucceededConsumer> _logger;

    public PaymentSucceededConsumer(IServiceScopeFactory scopeFactory, ILogger<PaymentSucceededConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task HandleAsync(PaymentSucceededIntegrationEvent message, CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.BookingId == message.BookingId,stoppingToken);

        if(booking == null) return;

        if(booking.Status == BookingStatuses.Confirmed)
        {
            _logger.LogInformation("items already booked");
            return;
        }
        if (!BookingStatusTransition.CanTransition(booking.Status, BookingStatuses.Confirmed))
        {
            _logger.LogWarning("Invalid status transition feom {Old} to confirmed for booking {Id}",booking.Status,booking.BookingId);
            return;
        }

        string oldStatus = booking.Status;
        booking.Status = BookingStatuses.Confirmed;

        db.BookingStatusHistories.Add(
            new BookingStatusHistory
            {
                HistoryId = Guid.NewGuid(),
                BookingId = booking.BookingId,
                PreviousStatus = oldStatus,
                NewStatus = BookingStatuses.Confirmed,
                ChangedBy = Guid.Empty,
                Reason = $"payment succeeded (paymentId : {message.PaymentId})"
            }
        );

        var ConfirmedEvent = new BookingConfirmedEvent(
            Guid.NewGuid(),
            booking.BookingId,
            booking.CustomerUserId,
            booking.HandOverMethod,
            booking.ReturnMethod,
            booking.StartTime,
            booking.EndTime,
            DateTime.Now
        );

        db.OutBoxMasaages.Add(new OutBoxMasaages
        {
            OutBoxMasaageId = ConfirmedEvent.EventId,
            EventType = nameof(BookingConfirmedEvent),
            Payload = JsonSerializer.Serialize(ConfirmedEvent)
        });

        await db.SaveChangesAsync(stoppingToken);
        
    }
}
