using System;

namespace BookingService.Model.Entity;

public class BookingPolicy
{
    public Guid BookingId {get; set;}

    public int FreeCancellationWindowHour{get; set;}

    public decimal LateFeePerHour {get; set;}

    public int GracePeriodInMinute{get; set;}

    public string Terms {get; set;} = "{}";

    public Booking? Booking {get; set;}

    

}
