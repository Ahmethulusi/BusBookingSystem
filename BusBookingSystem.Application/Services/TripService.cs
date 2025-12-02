// BusBookingSystem.Application/Services/TripService.cs
using BusBookingSystem.Application.DTOs;
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

            // 2. DTO'yu Entity'ye çevir
            var newTrip = new Trip
            {
                BusId = tripDto.BusId,
                Origin = tripDto.Origin,
                Destination = tripDto.Destination,
                DepartureDate = tripDto.DepartureDate,
                Price = tripDto.Price,
                // CreatedDate otomatik atanıyor (BaseEntity'den)
            };

            // 3. Veritabanına ekle
            await _context.Trips.AddAsync(newTrip);

            // 4. Değişiklikleri kaydet
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TripDto>> GetAllTripsAsync()
        {
            // Veritabanından tüm seferleri çek
            var trips = await _context.Trips
                .OrderBy(t => t.DepartureDate)
                .ToListAsync();

            // Entity'leri DTO'ya çevir (Mapping)
            return trips.Select(trip => new TripDto
            {
                Id = trip.Id,
                BusId = trip.BusId,
                Origin = trip.Origin,
                Destination = trip.Destination,
                DepartureDate = trip.DepartureDate,
                Price = trip.Price,
                CreatedDate = trip.CreatedDate
            });
        }
    }
}

