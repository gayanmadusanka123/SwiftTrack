using System;

namespace BookingService.Services;

public static class BookingStatusTransition
{
    private static readonly Dictionary<string, HashSet<string>> Allowed = new()
    {
        [BookingStatuses.Pending] = [BookingStatuses.PendingPayment, BookingStatuses.Rejected, BookingStatuses.Cancelled],
        [BookingStatuses.PendingPayment] = [BookingStatuses.Confirmed, BookingStatuses.Cancelled, BookingStatuses.Rejected],
        [BookingStatuses.Confirmed] = [BookingStatuses.ReadyForPickup, BookingStatuses.Active, BookingStatuses.Cancelled],
        [BookingStatuses.ReadyForPickup] = [BookingStatuses.Active, BookingStatuses.Cancelled],
        [BookingStatuses.Active] = [BookingStatuses.ReturnPending, BookingStatuses.Overdue, BookingStatuses.Completed],
        [BookingStatuses.Overdue] = [BookingStatuses.ReturnPending, BookingStatuses.Completed],
        [BookingStatuses.ReturnPending] = [BookingStatuses.Completed]


    };

    public static bool CanTransition(string currentStatus,string targetStatus)
    {
        return Allowed.TryGetValue(currentStatus,out var validTargets) && validTargets.Contains(targetStatus);
    }




}
