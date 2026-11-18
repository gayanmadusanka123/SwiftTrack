using System.Runtime.Serialization;

namespace BookingService.Contracts.Enum;

[DataContract(Namespace ="http://swiftrack.local.bookingservice")]
public enum HandOverMethodType
{
    [EnumMember] ShopDelivery = 0,
    [EnumMember] CustomerCollection = 1


}
