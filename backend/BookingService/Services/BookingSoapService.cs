using System;
using System.Text.Json;
using BookingService.ConnectedServices.Interfaces;
using BookingService.Contracts;
using BookingService.Contracts.Requests;
using BookingService.Contracts.Responses;
using BookingService.Kafka.Event;
using BookingService.Model;
using BookingService.Model.Entity;
using CoreWCF;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Services;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class BookingSoapService : IBookingSoapService
{
    private readonly BookingDbContext _dbContext;
    private readonly PricingCalculator _pricingCalculator;
    private readonly IverificationClient _verificationClient;
    private readonly IInventoryClient _inventoryClient;

    public BookingSoapService(
        BookingDbContext dbContext,
        PricingCalculator pricingCalculator,
        IverificationClient verificationClient,
        IInventoryClient inventoryClient
    )
    {
        _dbContext = dbContext;
        _pricingCalculator = pricingCalculator;
        _verificationClient = verificationClient;
        _inventoryClient = inventoryClient;

    }
    public async Task<BookingResponse> CancelBookingAsync(CancelBookingRequest request)
    {
        var booking = await _dbContext.Bookings
        .Include(b => b.Price)
        .FirstOrDefaultAsync(b => b.BookingId == request.BookingId);

        if (booking == null)
        {
            return new BookingResponse { Success = false, ErrorMessage = "Booking not found" };
        }

        if (!BookingStatusTransition.CanTransition(booking.Status, BookingStatuses.Cancelled))
        {
            return new BookingResponse
            {
                Success = false,
                BookingId = booking.BookingId,
                Status = booking.Status,
                ErrorMessage = $"Cannot cancel a booking in '{booking.Status}' status"
            };

        }

        string previousStatus = booking.Status;
        booking.Status = BookingStatuses.Cancelled;

        _dbContext.BookingStatusHistories.Add(new BookingStatusHistory
        {
            HistoryId = Guid.NewGuid(),
            BookingId = booking.BookingId,
            PreviousStatus = previousStatus,
            NewStatus = BookingStatuses.Cancelled,
            ChangedBy = request.ChangedBy,
            ChangedAt = DateTime.Now,
            Reason = request.Reason

        });

        var cancelEvent = new BookingCancelledEvent(
            Guid.NewGuid(),
            booking.BookingId,
            booking.CustomerUserId,
            request.Reason,
            DateTime.Now
        );

        _dbContext.OutBoxMasaages.Add(new OutBoxMasaages
        {
            OutBoxMasaageId = cancelEvent.EventId,
            EventType = nameof(BookingCancelledEvent),
            Payload = JsonSerializer.Serialize(cancelEvent)
        });

        await _dbContext.SaveChangesAsync();
        await _inventoryClient.ReleaseReservationAsync(booking.BookingId);

        return new BookingResponse
        {
            Success = true,
            BookingId = booking.BookingId,
            Status = booking.Status
        };

    }

    public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request)
    {
        if (request.EndTime <= request.StartTime)
        {
            return new BookingResponse { Success = false, ErrorMessage = "End time must be later than start time" };
        }
        if (request.Items == null || request.Items.Count == 0)
        {
            return new BookingResponse
            {
                Success = false,
                ErrorMessage = "Please enter the number of items required"
            };
        }

        //verify the customer identity
        var (isVerified, reason) = await _verificationClient.CheckCustomerEligibility(request.CustomerUserId);

        if (!isVerified)
        {
            return new BookingResponse
            {
                Success = false,
                Status = BookingStatuses.Rejected,
                ErrorMessage = reason ?? "Customer verification  failed"

            };

        }

        var bookingId = Guid.NewGuid();
        var unitIds = request.Items.Select(x => x.EquipmentUnitId).ToList();

        var (reserved, reservedError) = await _inventoryClient.ReservedUnitsAtomicalyAsync(bookingId, unitIds, request.StartTime, request.EndTime);

        if (!reserved)
        {
            return new BookingResponse
            {
                Success = false,
                Status = BookingStatuses.Rejected,
                ErrorMessage = reservedError ?? "Selected items are reserved"
            };

        }

        //calculate the price
        var priceCalc = _pricingCalculator.Calculate(request);

        var bookingItems = request.Items.Select(item => new BookingItem
        {
            BookingItemId = Guid.NewGuid(),
            BookingId = bookingId,
            EquipmentUnitId = item.EquipmentUnitId,
            DailyRate = item.DailyRentalRates,
            RentalAmount = item.DailyRentalRates * priceCalc.Billabledays,
            DepositAmount = item.DepositAmount
        }).ToList();

        var booking = new Booking
        {
            BookingId = bookingId,
            CustomerUserId = request.CustomerUserId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = BookingStatuses.PendingPayment,
            HandOverMethod = request.HandOverMethod.ToString(),
            ReturnMethod = request.ReturnMethod.ToString(),
            CreatedAt = DateTime.Now,
            Items = bookingItems,
            Price = new BookingPrice
            {
                BookingId = bookingId,
                Currency = "LKR",
                RentalSubTotal = priceCalc.RentalSubTotal,
                AdvancedPercent = priceCalc.AdvancePrecent,
                AdvancedAmount = priceCalc.AdvanceAmount,
                DepositTotal = priceCalc.DepositTotal,
                DeliveryFee = priceCalc.DeliveryFee,
                Discount = priceCalc.Discount,
                TotalDue = priceCalc.TotalDueAtBooking
            },
            Policy = new BookingPolicy
            {
                BookingId = bookingId,
                FreeCancellationWindowHour = 24,
                LateFeePerHour = 500.0m,
                GracePeriodInMinute = 60,
                Terms = "{\"AdvanceRefundableBeforeWindow\":true}"
            },
            StatusHistory = new List<BookingStatusHistory>
            {
                new()
                {
                    HistoryId = Guid.NewGuid(),
                    BookingId = bookingId,
                    PreviousStatus = BookingStatuses.Pending,
                    NewStatus = BookingStatuses.PendingPayment,
                    ChangedBy = request.CustomerUserId,
                    ChangedAt = DateTime.Now,
                    Reason = "Booking created and inventory temporary reserved"
                }
            }
        };

        var requestEvent = new BookingRequestedEvent(
            Guid.NewGuid(),
            bookingId,
            request.CustomerUserId,
            request.StartTime,
            request.EndTime,
            priceCalc.TotalDueAtBooking,
            unitIds,
            DateTime.Now
        );

        var outboxMessage = new OutBoxMasaages
        {
            OutBoxMasaageId = requestEvent.EventId,
            EventType = nameof(BookingRequestedEvent),
            Payload = JsonSerializer.Serialize(requestEvent)
        };

        _dbContext.Bookings.Add(booking);
        _dbContext.OutBoxMasaages.Add(outboxMessage);
        await _dbContext.SaveChangesAsync();

        return new BookingResponse
        {
            Success = true,
            BookingId = bookingId,
            Status = booking.Status,
            RentalSubTotal = priceCalc.RentalSubTotal,
            DepositTotal = priceCalc.DepositTotal,
            DeliveryFee = priceCalc.DeliveryFee,
            TotalDue = priceCalc.TotalDueAtBooking
        };



    }

    public async Task<BookingDetailsResponse> GetBookingDetailsAsync(GetBookingDetailsRequest request)
    {
        var booking = await _dbContext.Bookings
        .Include(b => b.Items)
        .Include(b => b.Price)
        .AsNoTracking()
        .FirstOrDefaultAsync(b => b.BookingId == request.BookingId);

        if (booking == null || booking.Price == null)
        {
            return new BookingDetailsResponse { Found = false };
        }

        return MapToDetailsResponse(booking);
    }

    public async Task<List<BookingDetailsResponse>> GetCustomerBookingAsync(Guid CustomerId)
    {
        var booking = await _dbContext.Bookings
       .Include(b => b.Items)
       .Include(b => b.Price)
       .AsNoTracking()
       .Where(b => b.CustomerUserId == CustomerId)
       .OrderByDescending(b => b.CreatedAt)
       .ToListAsync();

        return booking
        .Where(b => b.Price != null)
        .Select(MapToDetailsResponse)
        .ToList();

    }

    private static BookingDetailsResponse MapToDetailsResponse(Booking booking)
    {
        return new BookingDetailsResponse
        {
            Found = true,
            BookingId = booking.BookingId,
            CustomerUserId = booking.CustomerUserId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            HandoverMethod = booking.HandOverMethod,
            ReturnMethod = booking.ReturnMethod,
            Currency = booking.Price!.Currency,
            RentalSubtotal = booking.Price.RentalSubTotal,
            AdvancePercent = booking.Price.AdvancedPercent,
            AdvanceAmount = booking.Price.AdvancedAmount,
            DepositTotal = booking.Price.DepositTotal,
            DeliveryFee = booking.Price.DeliveryFee,
            Discount = booking.Price.Discount,
            TotalDue = booking.Price.TotalDue,
            Items = booking.Items.Select(i => new BookingItemResponseDto
            {
                BookingItemId = i.BookingItemId,
                EquipmentUnitId = i.EquipmentUnitId,
                DailyRentalRates = i.DailyRate,
                RentalAmount = i.RentalAmount,
                DepositAmount = i.DepositAmount
            }).ToList()
        };
    }
}

