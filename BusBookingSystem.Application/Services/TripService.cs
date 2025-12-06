// BusBookingSystem.Application/Services/TripService.cs
using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Application.Services
{
    public class TripService : ITripService
    {
        private readonly AppDbContext _context;

        public TripService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddTripAsync(CreateTripDto tripDto)
        {
            // 1. Bus'ın var olup olmadığını kontrol et
            var busExists = await _context.Buses.AnyAsync(b => b.Id == tripDto.BusId);
            if (!busExists)
            {
                throw new ArgumentException($"Bus with ID {tripDto.BusId} not found.");
            }

            // 2. Origin City'nin var olup olmadığını kontrol et
            var originCityExists = await _context.Cities.AnyAsync(c => c.Id == tripDto.OriginCityId);
            if (!originCityExists)
            {
                throw new ArgumentException($"Origin City with ID {tripDto.OriginCityId} not found.");
            }

            // 3. Destination City'nin var olup olmadığını kontrol et
            var destinationCityExists = await _context.Cities.AnyAsync(c => c.Id == tripDto.DestinationCityId);
            if (!destinationCityExists)
            {
                throw new ArgumentException($"Destination City with ID {tripDto.DestinationCityId} not found.");
            }

            // 4. Origin District kontrolü (eğer verilmişse)
            if (tripDto.OriginDistrictId.HasValue)
            {
                var originDistrictExists = await _context.Districts
                    .AnyAsync(d => d.Id == tripDto.OriginDistrictId.Value && d.CityId == tripDto.OriginCityId);
                if (!originDistrictExists)
                {
                    throw new ArgumentException($"Origin District with ID {tripDto.OriginDistrictId} not found in City {tripDto.OriginCityId}.");
                }
            }

            // 5. Destination District kontrolü (eğer verilmişse)
            if (tripDto.DestinationDistrictId.HasValue)
            {
                var destinationDistrictExists = await _context.Districts
                    .AnyAsync(d => d.Id == tripDto.DestinationDistrictId.Value && d.CityId == tripDto.DestinationCityId);
                if (!destinationDistrictExists)
                {
                    throw new ArgumentException($"Destination District with ID {tripDto.DestinationDistrictId} not found in City {tripDto.DestinationCityId}.");
                }
            }

            // 6. DTO'yu Entity'ye çevir
            var newTrip = new Trip
            {
                BusId = tripDto.BusId,
                OriginCityId = tripDto.OriginCityId,
                OriginDistrictId = tripDto.OriginDistrictId,
                DestinationCityId = tripDto.DestinationCityId,
                DestinationDistrictId = tripDto.DestinationDistrictId,
                DepartureDate = tripDto.DepartureDate,
                Price = tripDto.Price,
                // CreatedDate otomatik atanıyor (BaseEntity'den)
            };

            // 7. Veritabanına ekle
            await _context.Trips.AddAsync(newTrip);

            // 8. Değişiklikleri kaydet
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TripDto>> GetAllTripsAsync()
        {
            // Veritabanından tüm seferleri çek (City ve District bilgileriyle birlikte)
            var trips = await _context.Trips
                .Include(t => t.OriginCity)
                .Include(t => t.OriginDistrict)
                .Include(t => t.DestinationCity)
                .Include(t => t.DestinationDistrict)
                .OrderBy(t => t.DepartureDate)
                .ToListAsync();

            // Entity'leri DTO'ya çevir (Mapping)
            return trips.Select(trip => new TripDto
            {
                Id = trip.Id,
                BusId = trip.BusId,
                OriginCityId = trip.OriginCityId,
                OriginCityName = trip.OriginCity.Name,
                OriginDistrictId = trip.OriginDistrictId,
                OriginDistrictName = trip.OriginDistrict?.Name,
                DestinationCityId = trip.DestinationCityId,
                DestinationCityName = trip.DestinationCity.Name,
                DestinationDistrictId = trip.DestinationDistrictId,
                DestinationDistrictName = trip.DestinationDistrict?.Name,
                DepartureDate = trip.DepartureDate,
                Price = trip.Price,
                CreatedDate = trip.CreatedDate
            });
        }
    }
}

