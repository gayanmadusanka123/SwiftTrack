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
    public Task<BookingResponse> CancelBookingAsync(CancelBookingRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request)
    {
        throw new NotImplementedException();
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
