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
        _dbContext.outBoxMasaages.Add(outboxMessage);
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

        return new MapToDetailsResponse(booking);
    }

    public Task<List<BookingDetailsResponse>> GetCustomerBookingAsync(Guid CustomerId)
    {
        throw new NotImplementedException();
    }
}

internal class MapToDetailsResponse : BookingDetailsResponse
{
    private Booking booking;

    public MapToDetailsResponse(Booking booking)
    {
        this.booking = booking;
    }
}