using System;
using System.Runtime.Serialization;

namespace BookingService.Contracts.Responses;


[DataContract(Namespace = "http://swiftrack.local.bookingservice")]
public class BookingDetailsResponse
{

    [DataMember(Order = 1)]
    public bool Found { get; set; }

    [DataMember(Order = 2)]
    public Guid BookingId { get; set; }

    [DataMember(Order = 3)]
    public Guid CustomerUserId { get; set; }

    [DataMember(Order = 4)]
    public DateTime StartTime { get; set; }

    [DataMember(Order = 5)]
    public DateTime EndTime { get; set; }

    [DataMember(Order = 6)]
    public string Status { get; set; } = string.Empty;

    [DataMember(Order = 7)]
    public string HandoverMethod { get; set; } = string.Empty;

    [DataMember(Order = 8)]
    public string ReturnMethod { get; set; } = string.Empty;

    [DataMember(Order = 9)]
    public string Currency { get; set; } = "LKR";

    [DataMember(Order = 10)]
    public decimal RentalSubtotal { get; set; }

    [DataMember(Order = 11)]
    public decimal AdvancePercent { get; set; }

    [DataMember(Order = 12)]
    public decimal AdvanceAmount { get; set; }

    [DataMember(Order = 13)]
    public decimal DepositTotal { get; set; }

    [DataMember(Order = 14)]
    public decimal DeliveryFee { get; set; }

    [DataMember(Order = 15)]
    public decimal Discount { get; set; }

    [DataMember(Order = 16)]
    public decimal TotalDue { get; set; }

    [DataMember(Order = 17)]
    public List<BookingItemResponseDto> Items { get; set; } = new();



}

[DataContract(Namespace = "http://swiftrack.local.bookingservice")]
public class BookingItemResponseDto
{
    [DataMember(Order = 1)]
    public Guid BookingItemId { get; set; }

    [DataMember(Order = 2)]
    public Guid EquipmentUnitId { get; set; }

    [DataMember(Order = 3)]
    public decimal DailyRentalRates { get; set; }

    [DataMember(Order = 4)]
    public decimal RentalAmount { get; set; }

    [DataMember(Order = 5)]
    public decimal DepositAmount { get; set; }

}
