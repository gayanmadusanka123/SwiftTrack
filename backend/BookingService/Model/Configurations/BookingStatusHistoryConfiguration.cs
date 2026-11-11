using System;
using BookingService.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Model.Configurations;

public class BookingStatusHistoryConfiguration : IEntityTypeConfiguration<BookingStatusHistory>
{
    public void Configure(EntityTypeBuilder<BookingStatusHistory> builder)
    {
        builder.ToTable("BookingStatusHistory");
        builder.HasKey(h => h.HistoryId);

        builder.Property(h => h.PreviousStatus).HasMaxLength(32).IsRequired();
        builder.Property(h => h.NewStatus).HasMaxLength(32).IsRequired();
        builder.Property(h => h.Reason).HasMaxLength(512);

        builder.HasIndex(h => h.BookingId);
    }
}
