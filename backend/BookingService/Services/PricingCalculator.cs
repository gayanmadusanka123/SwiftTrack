using System;
using BookingService.Contracts.Enum;
using BookingService.Contracts.Requests;

namespace BookingService.Services;

public record PricingCalculatorResult(
    int Billabledays,
    decimal RentalSubTotal,
    decimal AdvancePrecent,
    decimal AdvanceAmount,
    decimal DepositTotal,
    decimal DeliveryFee,
    decimal Discount,
    decimal TotalDueAtBooking

);
public class PricingCalculator
{
    private readonly IConfiguration _config;

    public PricingCalculator(IConfiguration config)
    {
        _config = config;
    }

    public PricingCalculatorResult Calculate(CreateBookingRequest request)
    {
        decimal advancePrecent = _config.GetValue<decimal>("BookingSettings:AdvancePrecentage", 0.50m);
        decimal baseDeliveryFee = _config.GetValue<decimal>("BookingSettings:DeliveryFee", 1500.0m);

        int billabledays = Math.Max(1, (int)Math.Ceiling((request.EndTime - request.StartTime).TotalDays));

        decimal rentalSubTotal = 0m;
        decimal depositTotal = 0;

        foreach (var item in request.Items)
        {
            rentalSubTotal += item.DailyRentalRates * billabledays;
            depositTotal += item.DepositAmount;
        }

        decimal discount = Math.Clamp(request.DiscountAmount, 0m, rentalSubTotal);
        decimal netRental = rentalSubTotal - discount;
        decimal advanceAmount = Math.Round(netRental * advancePrecent, 2);

        decimal deliveryFee = 0m;

        if (request.HandOverMethod == HandOverMethodType.ShopDelivery)
        {
            deliveryFee += baseDeliveryFee;
        }
        if (request.ReturnMethod == ReturnMethodType.ShopPickup)
        {
            deliveryFee += baseDeliveryFee;
        }

        decimal totalDue = advanceAmount + depositTotal + deliveryFee;

        return new PricingCalculatorResult(
            billabledays,
            rentalSubTotal,
            advancePrecent,
            advanceAmount,
            depositTotal,
            deliveryFee,
            discount,
            totalDue
        );


    }


}
