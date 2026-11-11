using System;
using BookingService.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Model.Configurations;

public class BookingPolicyConfiguration : IEntityTypeConfiguration<BookingPolicy>
{
    public void Configure(EntityTypeBuilder<BookingPolicy> builder)
    {
        builder.ToTable("BookingPolicy");
        builder.HasKey(p => p.BookingId);

        builder.Property(p => p.LateFeePerHour).HasPrecision(18,2);
        builder.Property(p => p.Terms).HasMaxLength(4000);
    }
}
