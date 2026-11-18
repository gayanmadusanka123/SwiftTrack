using System;
using System.Runtime.Serialization;

namespace BookingService.Contracts.Requests;

[DataContract(Namespace ="http://swiftrack.local.bookingservice")]
public class CancelBookingRequest
{
    [DataMember(IsRequired = true, Order = 1)]
    public Guid BookingId { get; set; }

    [DataMember(IsRequired = true, Order = 2)]
    public Guid ChangedBy { get; set; }

    [DataMember(IsRequired = true, Order = 3)]
    public string Reason { get; set; } = string.Empty;

}
