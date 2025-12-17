using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class CityMapper
    {
        public static CityDto ToDto(this City city)
        {
            return new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                Districts = city.Districts?
                    .OrderBy(d => d.Name)
                    .Select(d => d.ToDto())
                    .ToList() ?? new List<DistrictDto>()
            };
        }

        public static IEnumerable<CityDto> ToDto(this IEnumerable<City> cities)
        {
            return cities.Select(city => city.ToDto());
        }
    }
}
