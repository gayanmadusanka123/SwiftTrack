using System;

namespace BookingService.Model.Entity;

public class BookingItem
{
    public Guid BookingItemId{get; set;}

    public Guid BookingId {get; set;}

    public Guid EquipmentUnitId {get; set;}

    public decimal DailyRate {get; set;}

    public decimal RentalAmount { get; set;}

    public decimal DepositAmount {get; set;}

    public Booking? Booking {get; set;}


}
