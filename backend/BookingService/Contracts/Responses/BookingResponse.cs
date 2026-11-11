using System;
using System.Runtime.Serialization;

namespace BookingService.Contracts.Responses;

[DataContract(Namespace ="http://swiftrack.local.bookingservice")]
public class BookingResponse
{
    [DataMember(Order = 1)]
    public bool Success {get; set;}

    [DataMember(Order = 2)]
    public string? ErrorMessage {get; set;}

    [DataMember(Order = 3)]
    public Guid? BookingId {get; set;}

    [DataMember(Order = 4)]
    public string? Status {get; set;}

    [DataMember(Order = 5)]
    public decimal RentalSubTotal {get; set;}

    [DataMember(Order = 6)]
    public decimal AdvanceAmount {get; set;}

    [DataMember(Order = 7)]
    public decimal DepositTotal {get; set;}

    [DataMember(Order = 8)]
    public decimal DeliveryFee {get; set;}

    [DataMember(Order = 9)]
    public decimal TotalDue {get; set;}

}
