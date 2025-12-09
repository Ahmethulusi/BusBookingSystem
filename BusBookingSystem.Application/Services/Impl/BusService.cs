// BusBookingSystem.Application/Services/BusService.cs
using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.Mappers;
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Application.Services.Impl
{
    public class BusService : IBusService
    {
        private readonly AppDbContext _context;

        public BusService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BusDto> AddBusAsync(CreateBusDto busDto)
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

            // 4. Oluşturulan otobüsü DTO olarak döndür
            return newBus.ToDto();
        }

        public async Task<IEnumerable<BusDto>> GetAllBusesAsync()
        {
            var buses = await _context.Buses
                .OrderBy(b => b.CreatedDate)
                .ToListAsync();

            return buses.ToDto();
        }
    }
}