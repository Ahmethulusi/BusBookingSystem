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
            // 1. Company'nin var olup olmadığını kontrol et
            var companyExists = await _context.Companies.AnyAsync(c => c.Id == busDto.CompanyId);
            if (!companyExists)
            {
                throw new ArgumentException($"Company with ID {busDto.CompanyId} not found.");
            }

            // 2. DTO'yu Entity'e çevir (Manuel Mapping)
            var newBus = new Bus
            {
                PlateNumber = busDto.PlateNumber,
                Brand = busDto.Brand,
                TotalSeatCount = busDto.TotalSeatCount,
                CompanyId = busDto.CompanyId,
            };

            // 3. Veritabanına ekle
            await _context.Buses.AddAsync(newBus);

            // 4. Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // 5. Oluşturulan otobüsü DTO olarak döndür
            return newBus.ToDto();
        }

        public async Task<IEnumerable<BusDto>> GetAllBusesAsync()
        {
            var buses = await _context.Buses
                .Include(b => b.Company)
                .OrderBy(b => b.CreatedDate)
                .ToListAsync();

            return buses.ToDto();
        }
    }
}