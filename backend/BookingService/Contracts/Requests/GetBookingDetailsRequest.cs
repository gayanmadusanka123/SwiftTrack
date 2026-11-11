using System;
using System.Runtime.Serialization;

namespace BookingService.Contracts.Requests;

[DataContract(Namespace ="http://swiftrack.local.bookingservice")]
public class GetBookingDetailsRequest
{
    [DataMember(IsRequired = true, Order =1)]
    public Guid BookingId{get; set;}

}
