using System.Runtime.Serialization;

namespace BookingService.Contracts.Enum;

[DataContract(Namespace ="http://swiftrack.local.bookingservice")]
public enum ReturnMethodType
{
    [EnumMember] ShopPickup = 0,
    [EnumMember] CustomerReturn = 1

}
