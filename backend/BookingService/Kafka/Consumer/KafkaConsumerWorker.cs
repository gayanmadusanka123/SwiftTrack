using System;
using System.Text.Json;
using BookingService.Kafka.Configuration;
using BookingService.Kafka.Publisher;
using Confluent.Kafka;

namespace BookingService.Kafka.Consumer;

public class KafkaConsumerWorker : BackgroundService
{

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly KafkaOptions _kafkaOptions;
    private readonly ILogger<OutBoxPublisher> _logger;

    public KafkaConsumerWorker(IServiceScopeFactory scopeFactory, KafkaOptions kafkaOptions, ILogger<OutBoxPublisher> logger)
    {
        _scopeFactory = scopeFactory;
        _kafkaOptions = kafkaOptions;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //run the blocking kafka consumer loop on a dedicated background thread
        return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
    }

    private async Task StartConsumerLoop(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServers,
            GroupId = _kafkaOptions.GroupId,
            AutoOffsetReset = Enum.TryParse<AutoOffsetReset>(_kafkaOptions.AutoOffsetReset, true, out var reset)
           ? reset
           : AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();

        var topics = new[]
        {
            _kafkaOptions.Topics.PaymentSucceeded,
            _kafkaOptions.Topics.PaymentFailed,
            _kafkaOptions.Topics.ReservedExpired
        };

        consumer.Subscribe(topics);
        _logger.LogInformation("Kafka Consumer subscribed to topics :{Topics}", string.Join(", ", topics));

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    if (result?.Message == null)
                        continue;

                    _logger.LogInformation("Recieved Kafka msg from topics {Topic} [Partition : {Partition},Offeset : {Offset}]",
                    result.Topic, result.Partition.Value, result.Offset.Value);

                    await DispatchMessageAsync(result.Topic, result.Message.Value, stoppingToken);

                    //commit offset only after successfully db processing
                    consumer.Commit(result);
                }
                catch (ConsumeException ex)
                {

                    _logger.LogError(ex, "kafka consume error {Reason}", ex.Error.Reason);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing consumed kafka message");
                }
            }
        }
        finally
        {

            consumer.Close();
        }
    }

    private async Task DispatchMessageAsync(string topic, string payload, CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        if (topic == _kafkaOptions.Topics.PaymentSucceeded)
        {
            var evt = JsonSerializer.Deserialize<PaymentSucceededIntegrationEvent>(payload);

            if (evt != null)
            {
                var handler = scope.ServiceProvider.GetRequiredService<PaymentSucceededConsumer>();
                await handler.HandleAsync(evt, stoppingToken);

            }
        }
        else if (topic == _kafkaOptions.Topics.PaymentFailed)
        {
            var evt = JsonSerializer.Deserialize<PaymentFailedIntegrationEvent>(payload);

            if (evt != null)
            {
                var handler = scope.ServiceProvider.GetRequiredService<PaymentFailedConsumer>();
                await handler.HandleAsync(evt, stoppingToken);

            }
        }
        else if (topic == _kafkaOptions.Topics.ReservedExpired)
        {
            var evt = JsonSerializer.Deserialize<ReservationExpiredIntegrationEvent>(payload);

            if (evt != null)
            {
                var handler = scope.ServiceProvider.GetRequiredService<ReservationExpiredConsumer>();
                await handler.HandleAsync(evt, stoppingToken);

            }

        }


    }
}
