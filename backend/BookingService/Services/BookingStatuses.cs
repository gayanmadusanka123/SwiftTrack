using System;

namespace BookingService.Services;

public static class BookingStatuses
{
    public const string Pending = "Pending";
    public const string PendingPayment = "PendingPayment";
    public const string Confirmed = "Confirmed";
    public const string ReadyForPickup = "ReadyForPikcUp";
    public const string Active = "Active";
    public const string ReturnPending = "ReturnPending";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
    public const string Rejected = "Rejected";
    public const string Overdue = "Overdue";

}
