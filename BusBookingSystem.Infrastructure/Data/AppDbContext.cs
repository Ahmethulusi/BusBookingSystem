// Dosya: BusBookingSystem.Infrastructure/Data/AppDbContext.cs
using BusBookingSystem.Core.Entities;
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
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        // Veritabanı oluşurken çalışacak özel ayarlar
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Örneğin: Bir otobüs silinirse seferleri ne olsun?
            // Restrict: Otobüs silinmeye çalışılırsa ve seferi varsa izin verme (Güvenli olan)
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Bus)
                .WithMany(b => b.Trips)
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}