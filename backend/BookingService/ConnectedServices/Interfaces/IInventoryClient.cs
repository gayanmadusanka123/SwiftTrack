using System;
using System.Globalization;

namespace BookingService.ConnectedServices.Interfaces;

public interface IInventoryClient
{
    Task<(bool Reserved,string? ErrorMessage)> ReservedUnitsAtomicalyAsync(
        Guid bookingId,
        List<Guid> equipmentUnitId,
        DateTime StartTime,
        DateTime EndTime,
        CancellationToken ct = default
    );

    Task ReleaseReservationAsync(
        Guid bookingId,
        CancellationToken ct = default
    );

}
