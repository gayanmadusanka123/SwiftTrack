using System;
using System.Runtime.Serialization;
using BookingService.Contracts.Enum;

namespace BookingService.Contracts.Requests;

[DataContract(Namespace = "http://swiftrack.local.bookingservice")]
public class CreateBookingRequest
{
    [DataMember(IsRequired = true, Order = 1)]
    public Guid CustomerUserId { get; set; }

    [DataMember(IsRequired = true, Order = 2)]
    public DateTime StartTime { get; set; }

    [DataMember(IsRequired = true, Order = 3)]
    public DateTime EndTime { get; set; }

    [DataMember(IsRequired = true, Order = 4)]
    public HandOverMethodType HandOverMethod { get; set; } 

    [DataMember(IsRequired = true, Order = 5)]
    public ReturnMethodType ReturnMethod { get; set; }

    [DataMember(IsRequired = false, Order = 6)]
    public decimal DiscountAmount{get; set;}

    [DataMember(IsRequired = true, Order = 7)]
    public List<BookingItemRequestDto> Items {get; set;} = [];



}

[DataContract(Namespace ="http://swiftrack.local.bookingservice")]
public class BookingItemRequestDto
{
    [DataMember(IsRequired = true, Order = 1)]
    public Guid EquipmentUnitId {get; set;}

    [DataMember(IsRequired = true, Order = 2)]
    public decimal DailyRentalRates {get; set;}

    [DataMember(IsRequired = true, Order = 3)]
    public decimal DepositAmount {get; set;}
    
}


