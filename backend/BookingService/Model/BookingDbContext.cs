using System;
using BookingService.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Model;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) { }

    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingItem> BookingItems => Set<BookingItem>();
    public DbSet<BookingPrice> BookingPrices => Set<BookingPrice>();
    public DbSet<BookingPolicy> BookingPolicies => Set<BookingPolicy>();
    public DbSet<BookingStatusHistory> BookingStatusHistories => Set<BookingStatusHistory>();
    public DbSet<OutBoxMasaages> OutBoxMasaages => Set<OutBoxMasaages>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);

        modelBuilder.Entity<OutBoxMasaages>(b =>
        {
            b.ToTable("OutboxMassages");
            b.HasKey(x => x.OutBoxMasaageId);
            b.Property(x => x.EventType).HasMaxLength(128).IsRequired();
            b.HasIndex(x => x.ProcessedAt);
        });


    }

}
