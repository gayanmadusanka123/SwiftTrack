using System.Text;
using BookingService.Kafka.Configuration;
using BookingService.Model;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;

//Reads unprocessed records from OutboxMessages and publishes them to the swifttrack.booking.events Kafka topic with the EventType header and OutboxMessageId as the message key

namespace BookingService.Kafka.Publisher;

public class OutBoxPublisher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly KafkaOptions _kafkaOptions;
    private readonly ILogger<OutBoxPublisher> _logger;
    private IProducer<string, string> _producer;

    public OutBoxPublisher(IServiceScopeFactory scopeFactory, KafkaOptions kafkaOptions, ILogger<OutBoxPublisher> logger)
    {
        _scopeFactory = scopeFactory;
        _kafkaOptions = kafkaOptions;
        _logger = logger;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            MessageSendMaxRetries = 3
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

                var pendingMessages = await db.outBoxMasaages
                .Where(m => m.ProcessedAt == null)
                .OrderBy(m => m.CreatedAt)
                .Take(20)
                .ToListAsync(stoppingToken);

                foreach (var message in pendingMessages)
                {
                    try
                    {
                        var kafkaMessage = new Message<string, string>
                        {
                            Key = message.OutBoxMasaageId.ToString(),
                            Value = message.Payload,
                            Headers = new Headers
                            {
                                {"event-type",Encoding.UTF8.GetBytes(message.EventType)}
                            }
                        };

                        var deliveryResult = await _producer.ProduceAsync(
                            _kafkaOptions.Topics.BookingEvents,
                            kafkaMessage,
                            stoppingToken
                        );

                        message.ProcessedAt = DateTime.Now;
                        message.Error = null;

                        _logger.LogInformation(
                            "Publshed {EventType} {MessageId} to kafka topic {topic} [Partition: {Partition}, Offset :{Offset}]",
                            message.EventType,
                            message.OutBoxMasaageId,
                            deliveryResult.Topic,
                            deliveryResult.Partition.Value,
                            deliveryResult.Offset.Value
                        );
                    }
                    catch (ProduceException<string,string> ex)
                    {
                        message.Error = ex.Error.Reason;
                        _logger.LogError(ex,"kafka failde to outbox message {MessageId}",message.OutBoxMasaageId);

                    }
                }

                if(pendingMessages.Count > 0)
                {
                    await db.SaveChangesAsync(stoppingToken);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"error polling outbox message");
            }

            await Task.Delay(TimeSpan.FromSeconds(5),stoppingToken);
        }
    }

    public override void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
        base.Dispose();
    }
}
