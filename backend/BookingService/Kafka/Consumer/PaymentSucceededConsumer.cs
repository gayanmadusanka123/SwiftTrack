using System;
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
        
        
    }
}
