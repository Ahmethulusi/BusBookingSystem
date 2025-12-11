// BusBookingSystem.Application/Services/TripService.cs
using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Mappers;
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Application.Services.Impl
{
    public class TripService : ITripService
    {
        private readonly AppDbContext _context;

        public TripService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TripDto> AddTripAsync(CreateTripDto tripDto)
        {
            // 1. Company'nin var olup olmadığını kontrol et
            var companyExists = await _context.Companies.AnyAsync(c => c.Id == tripDto.CompanyId);
            if (!companyExists)
            {
                throw new ArgumentException($"Company with ID {tripDto.CompanyId} not found.");
            }

            // 2. Bus'ın var olup olmadığını ve Company'ye ait olup olmadığını kontrol et
            var bus = await _context.Buses
                .FirstOrDefaultAsync(b => b.Id == tripDto.BusId && b.CompanyId == tripDto.CompanyId);
            if (bus == null)
            {
                var busExists = await _context.Buses.AnyAsync(b => b.Id == tripDto.BusId);
                if (!busExists)
                {
                    throw new ArgumentException($"Bus with ID {tripDto.BusId} not found.");
                }
                else
                {
                    throw new ArgumentException($"Bus with ID {tripDto.BusId} does not belong to Company with ID {tripDto.CompanyId}.");
                }
            }

            // 3. Origin City'nin var olup olmadığını kontrol et
            var originCityExists = await _context.Cities.AnyAsync(c => c.Id == tripDto.OriginCityId);
            if (!originCityExists)
            {
                throw new ArgumentException($"Origin City with ID {tripDto.OriginCityId} not found.");
            }

            // 4. Destination City'nin var olup olmadığını kontrol et
            var destinationCityExists = await _context.Cities.AnyAsync(c => c.Id == tripDto.DestinationCityId);
            if (!destinationCityExists)
            {
                throw new ArgumentException($"Destination City with ID {tripDto.DestinationCityId} not found.");
            }

            // 5. Origin District kontrolü (eğer verilmişse)
            if (tripDto.OriginDistrictId.HasValue)
            {
                var originDistrictExists = await _context.Districts
                    .AnyAsync(d => d.Id == tripDto.OriginDistrictId.Value && d.CityId == tripDto.OriginCityId);
                if (!originDistrictExists)
                {
                    throw new ArgumentException($"Origin District with ID {tripDto.OriginDistrictId} not found in City {tripDto.OriginCityId}.");
                }
            }

            // 6. Destination District kontrolü (eğer verilmişse)
            if (tripDto.DestinationDistrictId.HasValue)
            {
                var destinationDistrictExists = await _context.Districts
                    .AnyAsync(d => d.Id == tripDto.DestinationDistrictId.Value && d.CityId == tripDto.DestinationCityId);
                if (!destinationDistrictExists)
                {
                    throw new ArgumentException($"Destination District with ID {tripDto.DestinationDistrictId} not found in City {tripDto.DestinationCityId}.");
                }
            }

            // 7. DTO'yu Entity'ye çevir
            var newTrip = new Trip
            {
                CompanyId = tripDto.CompanyId,
                BusId = tripDto.BusId,
                OriginCityId = tripDto.OriginCityId,
                OriginDistrictId = tripDto.OriginDistrictId,
                DestinationCityId = tripDto.DestinationCityId,
                DestinationDistrictId = tripDto.DestinationDistrictId,
                DepartureDate = tripDto.DepartureDate,
                DepartureTime = tripDto.DepartureTime,
                Price = tripDto.Price
            };

            // 8. Veritabanına ekle
            await _context.Trips.AddAsync(newTrip);

            // 9. Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // 10. Oluşturulan seferi ilişkili verilerle birlikte getir
            var createdTrip = await _context.Trips
                .Include(t => t.OriginCity)
                .Include(t => t.OriginDistrict)
                .Include(t => t.DestinationCity)
                .Include(t => t.DestinationDistrict)
                .FirstOrDefaultAsync(t => t.Id == newTrip.Id);

            // 11. DTO'ya çevir ve döndür
            return createdTrip!.ToDto();
        }

        public async Task<IEnumerable<TripDto>> GetAllTripsAsync()
        {
            var today = DateTime.Today;

            var trips = await _context.Trips
                .Include(t => t.OriginCity)
                .Include(t => t.OriginDistrict)
                .Include(t => t.DestinationCity)
                .Include(t => t.DestinationDistrict)
                .Where(t => t.DepartureDate >= today)
                .OrderBy(t => t.DepartureDate)
                .ThenBy(t => t.DepartureTime)
                .ToListAsync();

            return trips.ToDto();
        }
    }
}

