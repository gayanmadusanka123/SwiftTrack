using System;

namespace BookingService.Kafka.Configuration;

public class KafkaOptions
{
    public const string SectionName = "Kafka";
    
    public string BootstrapServers {get; set;} = "localhost:9092";
    public string GroupId {get; set;} = "swifttrack-booking-service-group";

    public string AutoOffsetReser {get; set;} = "Earliest";

    public KafkaTopics Topics {get; set;} = new();
    

}

public class KafkaTopics
{
    public string BookingEvents {get; set;} = "swifttrack.booking.payment";
    public string PaymentSucceeded {get; set;} = "swifttrack.payment.succeeded";

    public string PaymentFailed {get; set;} = "swifttrack.payment.failed";

    public string ReservedExpired {get; set;} = "swittrack.inventory.reservation.expired";

}