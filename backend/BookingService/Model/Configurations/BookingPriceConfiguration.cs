using System;
using BookingService.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Model.Configurations;

public class BookingPriceConfiguration : IEntityTypeConfiguration<BookingPrice>
{
    public void Configure(EntityTypeBuilder<BookingPrice> builder)
    {
        builder.ToTable("BookingPrice");
        builder.HasKey(p => p.BookingId);

        builder.Property(p => p.Currency).HasMaxLength(8).IsRequired();
        builder.Property(p => p.RentalSubTotal).HasPrecision(18, 2);
        builder.Property(p => p.AdvancedPercent).HasPrecision(18, 2);
        builder.Property(p => p.AdvancedAmount).HasPrecision(18, 2);
        builder.Property(p => p.DepositTotal).HasPrecision(18, 2);
        builder.Property(p => p.DeliveryFee).HasPrecision(18, 2);
        builder.Property(p => p.Discount).HasPrecision(18, 2);
        builder.Property(p => p.TotalDue).HasPrecision(18, 2);

    }
}
