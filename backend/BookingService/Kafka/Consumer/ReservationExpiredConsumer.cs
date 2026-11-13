using System;
using BookingService.Model;
using BookingService.Model.Entity;
using BookingService.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Kafka.Consumer;

public record ReservationExpiredIntegrationEvent(Guid EventId, Guid BookingId);
public class ReservationExpiredConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReservationExpiredConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    internal async Task HandleAsync(ReservationExpiredIntegrationEvent message, CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.BookingId == message.BookingId, stoppingToken);

        if (booking == null || booking.Status != BookingStatuses.PendingPayment) return;



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
                Reason = $"Reservation expired"
            }
        );



        await db.SaveChangesAsync(stoppingToken);
    }
}
