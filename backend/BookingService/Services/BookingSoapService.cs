using System;
using BookingService.ConnectedServices.Interfaces;
using BookingService.Contracts;
using BookingService.Contracts.Requests;
using BookingService.Contracts.Responses;
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
        


    }

    public Task<BookingDetailsResponse> GetBookingDetailsAsync(GetBookingDetailsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<List<BookingDetailsResponse>> GetCustomerBookingAsync(Guid CustomerId)
    {
        throw new NotImplementedException();
    }
}
