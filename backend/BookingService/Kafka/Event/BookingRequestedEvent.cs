using System;

namespace BookingService.Kafka.Event;

public record BookingRequestedEvent(
    Guid EventId,
    Guid BookingId,
    Guid CustomerUserId,
    DateTime StartTime,
    DateTime EndTime,
    decimal TotalDue,
    List<Guid> EquipmentUnitIds,
    DateTime OccurredAt);
