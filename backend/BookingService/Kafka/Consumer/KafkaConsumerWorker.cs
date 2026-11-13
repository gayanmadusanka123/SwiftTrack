using System;
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
                    
                }
                catch (System.Exception)
                {
                    
                    throw;
                }
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}
