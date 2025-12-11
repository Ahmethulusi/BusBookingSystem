// Dosya: BusBookingSystem.Infrastructure/Data/AppDbContext.cs
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Infrastructure.Data
{
    // Bu sınıf veritabanının ta kendisidir.
    public class AppDbContext : DbContext
    {
        // Constructor: Ayarları (Connection String vb.) alıp mirasa (base) gönderir.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tablo tanımları
        public DbSet<Company> Companies { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<User> Users { get; set; }

        // Veritabanı oluşurken çalışacak özel ayarlar
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Örneğin: Bir otobüs silinirse seferleri ne olsun?
            // Restrict: Otobüs silinmeye çalışılırsa ve seferi varsa izin verme (Güvenli olan)

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

            // Ticket unique constraint (bir seferde aynı koltuk sadece bir kez rezerve edilebilir)
            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.TripId, t.SeatNumber })
                .IsUnique();

            // User unique constraint
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // City - District relationship (One-to-Many)
            modelBuilder.Entity<District>()
                .HasOne(d => d.City)
                .WithMany(c => c.Districts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Trip - City relationships (Origin)
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

            // Trip - City relationships (Destination)
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

            base.OnModelCreating(modelBuilder);
        }

        // Tüm entity'ler için CreatedDate ve UpdatedDate'i otomatik olarak Türkiye saatine göre ayarla
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
                    // Eğer CreatedDate set edilmemişse, Türkiye saatini kullan
                    if (entity.CreatedDate == default(DateTime))
                    {
                        entity.CreatedDate = DateTimeHelper.GetTurkeyTimeNow();
                    }
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    // UpdatedDate'i her zaman Türkiye saatine göre güncelle
                    entity.UpdatedDate = DateTimeHelper.GetTurkeyTimeNow();
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}