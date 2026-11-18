using System;
using BookingService.ConnectedServices.Interfaces;

namespace BookingService.ConnectedServices.implementations;

public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<InventoryClient> _logger;

    public InventoryClient(HttpClient httpClient, ILogger<InventoryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task ReleaseReservationAsync(Guid bookingId, CancellationToken ct = default)
    {

        _logger.LogInformation("Releasing temporary incventory reservations for booking {bookingId}",bookingId);
        await Task.CompletedTask;
    }

    public async Task<(bool Reserved, string? ErrorMessage)> ReservedUnitsAtomicalyAsync(Guid bookingId, List<Guid> equipmentUnitId, DateTime StartTime, DateTime EndTime, CancellationToken ct = default)
    {
        try
        {
            await Task.CompletedTask;
            return (true,null);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Inventory Reservation failed for booking {bookingid}",bookingId);
            return (false,"could not reserve equipment units in Inventory service");
            
        }
    }
}
