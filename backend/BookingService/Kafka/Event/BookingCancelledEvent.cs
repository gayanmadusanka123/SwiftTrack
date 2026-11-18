using System;

namespace BookingService.Kafka.Event;

public  record BookingCancelledEvent(
    Guid EventId,
    Guid BookingId,
    Guid CustomerUserId,
    string Reason,
    DateTime CancelledAt
);
