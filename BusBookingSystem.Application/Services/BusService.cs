// BusBookingSystem.Application/Services/BusService.cs
using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Infrastructure.Data;

namespace BusBookingSystem.Application.Services
{
    public class BusService : IBusService
    {
        private readonly AppDbContext _context;

        // Dependency Injection ile DbContext'i istiyoruz
        public BusService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBusAsync(CreateBusDto busDto)
        {
            // 1. DTO'yu Entity'e çevir (Manuel Mapping)
            var newBus = new Bus
            {
                PlateNumber = busDto.PlateNumber,
                Brand = busDto.Brand,
                TotalSeatCount = busDto.TotalSeatCount,
                // CreatedDate otomatik atanıyor (BaseEntity'den)
            };

            // 2. Veritabanına ekle
            await _context.Buses.AddAsync(newBus);
            
            // 3. Değişiklikleri kaydet
            await _context.SaveChangesAsync();
        }
    }
}