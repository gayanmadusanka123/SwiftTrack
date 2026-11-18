using System;
using BookingService.ConnectedServices.Interfaces;

namespace BookingService.ConnectedServices.implementations;

public class VerificationClient : IverificationClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<VerificationClient> _logger;

    public VerificationClient(HttpClient httpClient ,ILogger<VerificationClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<(bool IsVerified, string? Reason)> CheckCustomerEligibility(Guid CustomerId, CancellationToken ct = default)
    {
        try
        {
            //call verification service when available
            await Task.CompletedTask;
            return (true,null);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Failed to verify customer {CustomerId}",CustomerId);
            return (false,"Verifications service unavailable");
        }
        
    }
}
