using System;

namespace BookingService.ConnectedServices.Interfaces;

public interface IverificationClient
{
    Task<(bool IsVerified, string? Reason)> CheckCustomerEligibility(Guid CustomerId,CancellationToken ct = default);

}
