// BusBookingSystem.Application/Services/LocationService.cs
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Mappers;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Application.Services.Impl
{
    public class LocationService : ILocationService
    {
        private readonly AppDbContext _context;

        public LocationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
        {
            var cities = await _context.Cities
                .Include(c => c.Districts)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return cities.ToDto();
        }

        public async Task<IEnumerable<DistrictDto>> GetDistrictsByCityIdAsync(int cityId)
        {
            var cityExists = await _context.Cities.AnyAsync(c => c.Id == cityId);
            if (!cityExists)
            {
                throw new ArgumentException($"City with ID {cityId} not found.");
            }

            var districts = await _context.Districts
                .Where(d => d.CityId == cityId)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return districts.ToDto();
        }
    }
}

