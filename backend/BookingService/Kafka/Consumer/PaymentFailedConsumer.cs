using System;
using BookingService.ConnectedServices.Interfaces;
using BookingService.Kafka.Event;
using BookingService.Model;
using BookingService.Model.Entity;
using BookingService.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Kafka.Consumer;

public record PaymentFailedIntegrationEvent(Guid EventId, Guid BookingId, string FailureReason);
public class PaymentFailedConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IInventoryClient _inventoryClient;

    public PaymentFailedConsumer(IServiceScopeFactory scopeFactory, IInventoryClient inventoryClient)
    {
        _scopeFactory = scopeFactory;
        _inventoryClient = inventoryClient;
    }

    internal async Task HandleAsync(PaymentFailedIntegrationEvent message, CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.BookingId == message.BookingId,stoppingToken);

        if(booking == null || booking.Status != BookingStatuses.PendingPayment) return;

        string oldStatus = booking.Status;
        booking.Status = BookingStatuses.Cancelled;

        db.bookingStatusHistories.Add(
            new BookingStatusHistory
            {
                HistoryId = Guid.NewGuid(),
                BookingId = booking.BookingId,
                PreviousStatus = oldStatus,
                NewStatus = BookingStatuses.Cancelled,
                ChangedBy = Guid.Empty,
                Reason = $"payment failed  : {message.FailureReason}"
            }
        );

       

       ;

        await db.SaveChangesAsync(stoppingToken);
        await _inventoryClient.ReleaseReservationAsync(booking.BookingId,stoppingToken);
    }
}
