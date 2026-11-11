using System;

namespace BookingService.Model.Entity;

public class BookingPrice
{
    public Guid BookingId {get; set;}

    public string Currency {get; set;}= "LKR";

    public decimal RentalSubTotal {get; set;}

    public decimal AdvancedPercent {get; set;}

    public decimal AdvancedAmount {get; set;}

    public decimal DepositTotal {get; set;}

    public decimal DeliveryFee {get; set;} 

    public decimal Discount {get; set;}

    public decimal TotalDue {get; set;}

    public Booking? Booking{get; set;}

}
