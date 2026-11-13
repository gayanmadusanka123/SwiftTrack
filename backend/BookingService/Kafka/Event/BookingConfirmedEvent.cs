using System;

namespace BookingService.Kafka.Event;

public record BookingConfirmedEvent(
    Guid EventId,
    Guid BookingId,
    Guid CustomerUserId,
    string HandoverMethod,
    string ReturnMethod,
    DateTime StartTime,
    DateTime EndTime,
    DateTime ConfirmedAt);
