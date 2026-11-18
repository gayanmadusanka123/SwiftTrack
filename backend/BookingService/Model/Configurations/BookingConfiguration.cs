using System;
using BookingService.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingService.Model.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure (EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(b => b.BookingId);

        builder.Property(b => b.Status).HasMaxLength(32).IsRequired();
        builder.Property(b => b.HandOverMethod).HasMaxLength(32).IsRequired();
        builder.Property(b => b.ReturnMethod).HasMaxLength(32).IsRequired();

        builder.HasIndex(b => b.CustomerUserId);
        builder.HasIndex(b => new {b.Status,b.StartTime,b.EndTime});

        builder.HasMany(b => b.Items)
        .WithOne(p => p.Booking)
        .HasForeignKey(i => i.BookingId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Price)
        .WithOne(p => p.Booking)
        .HasForeignKey<BookingPrice>(p=> p.BookingId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Policy)
        .WithOne(p => p.Booking)
        .HasForeignKey<BookingPolicy>(p=> p.BookingId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.StatusHistory)
        .WithOne(h => h.Booking)
        .HasForeignKey(h => h.BookingId)
        .OnDelete(DeleteBehavior.Cascade);
        
        
    }

}
