using System;

namespace BookingService.Kafka.Consumer;

public class ReservationExpiredConsumer
{
    internal async Task HandleAsync(ReservationExpiredIntergrationEvent evt, CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
