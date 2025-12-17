// Dosya: BusBookingSystem.Infrastructure/Data/AppDbContext.cs
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; // ✨ BU EKLENDİ

namespace BusBookingSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // İlişkiler

            // Company - Bus relationship (One-to-Many)
            modelBuilder.Entity<Bus>()
                .HasOne(b => b.Company)
                .WithMany(c => c.Buses)
                .HasForeignKey(b => b.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Company - Trip relationship (One-to-Many)
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Company)
                .WithMany(c => c.Trips)
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Bus - Trip relationship (One-to-Many)
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Bus)
                .WithMany()
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Trip - Ticket relationship (One-to-Many)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Trip)
                .WithMany(tr => tr.Tickets)
                .HasForeignKey(t => t.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            // Passenger - Ticket relationship (One-to-Many)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Passenger)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PassengerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Passenger unique constraints
            modelBuilder.Entity<Passenger>()
                .HasIndex(p => p.TcNo)
                .IsUnique();

            modelBuilder.Entity<Passenger>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // Ticket unique constraint
            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.TripId, t.SeatNumber })
                .IsUnique();

            // User unique constraint
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // City - District relationship
            modelBuilder.Entity<District>()
                .HasOne(d => d.City)
                .WithMany(c => c.Districts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Trip - City/District relationships
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.OriginCity)
                .WithMany()
                .HasForeignKey(t => t.OriginCityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.OriginDistrict)
                .WithMany()
                .HasForeignKey(t => t.OriginDistrictId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.DestinationCity)
                .WithMany()
                .HasForeignKey(t => t.DestinationCityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.DestinationDistrict)
                .WithMany()
                .HasForeignKey(t => t.DestinationDistrictId)
                .OnDelete(DeleteBehavior.Restrict);


            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));

            var timeOnlyConverter = new ValueConverter<TimeOnly, TimeSpan>(
                t => t.ToTimeSpan(),
                t => TimeOnly.FromTimeSpan(t));

            // Projedeki tüm Entity'leri gez ve bu tipleri görünce çeviriciyi uygula
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateOnly))
                    {
                        property.SetValueConverter(dateOnlyConverter);
                    }
                    else if (property.ClrType == typeof(TimeOnly))
                    {
                        property.SetValueConverter(timeOnlyConverter);
                    }
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    if (entity.CreatedDate == default(DateTime))
                    {
                        entity.CreatedDate = DateTimeHelper.GetTurkeyTimeNow();
                    }
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.UpdatedDate = DateTimeHelper.GetTurkeyTimeNow();
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}