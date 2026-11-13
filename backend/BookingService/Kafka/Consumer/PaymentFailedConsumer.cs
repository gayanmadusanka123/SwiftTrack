using System;

namespace BookingService.Kafka.Consumer;

public class PaymentFailedConsumer
{
    internal async Task HandleAsync(PaymentFailedIntergrationEvent evt, CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}
