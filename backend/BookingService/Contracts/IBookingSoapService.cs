using System;
using BookingService.Contracts.Requests;
using BookingService.Contracts.Responses;
using CoreWCF;

namespace BookingService.Contracts;

[ServiceContract(Namespace = "http://swifttrack.local/bookingservice")]
public interface IBookingSoapService
{
    [OperationContract]
    Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);

    [OperationContract]
    Task<BookingDetailsResponse> GetBookingDetailsAsync(GetBookingDetailsRequest request);

    [OperationContract]
    Task<BookingResponse> CancelBookingAsync(CancelBookingRequest request);

    [OperationContract]
    Task<List<BookingDetailsResponse>> GetCustomerBookingAsync(Guid CustomerId);
    

}
