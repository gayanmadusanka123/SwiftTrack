using System;

namespace BookingService.Model.Entity;

public class BookingStatusHistory
{
    public Guid HistoryId { get; set; }
    public Guid BookingId { get; set; }

    public string PreviousStatus { get; set; } = string.Empty;

    public string NewStatus { get; set; } = string.Empty;

    public Guid ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.Now;

    public string Reason { get; set; } = string.Empty;

    public Booking? Booking { get; set; }


}
