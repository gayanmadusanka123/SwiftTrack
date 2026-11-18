using System;
using BookingService.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Model.Configurations;

public class BookingItemConfiguration : IEntityTypeConfiguration<BookingItem>
{
    public void Configure(EntityTypeBuilder<BookingItem> builder)
    {
        builder.ToTable("BookingItems");
        builder.HasKey(i => i.BookingItemId);

        builder.Property(i => i.DailyRate).HasPrecision(18,2);
        builder.Property(i => i.RentalAmount).HasPrecision(18,2);
        builder.Property(i => i.DepositAmount).HasPrecision(18,2);

        builder.HasIndex(i => i.EquipmentUnitId);
    }
}
