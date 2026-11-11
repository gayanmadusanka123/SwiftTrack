using System;

namespace BookingService.Model.Entity;

public class Booking
{
    public Guid BookingId { get; set; }
    public Guid CustomerUserId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Status { get; set; } = "Pending Payment";

    public string HandOverMethod { get; set; } = string.Empty;

    public string ReturnMethod { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<BookingItem> Items { get; set; } = [];
    public BookingPrice? Price { get; set; }
    public BookingPolicy? Policy { get; set; }

    public List<BookingStatusHistory> StatusHistory { get; set; } = [];


}
