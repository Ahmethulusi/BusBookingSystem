using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class DistrictMapper
    {
        public static DistrictDto ToDto(this District district)
        {
            return new DistrictDto
            {
                Id = district.Id,
                CityId = district.CityId,
                Name = district.Name
            };
        }

        public static IEnumerable<DistrictDto> ToDto(this IEnumerable<District> districts)
        {
            return districts.Select(district => district.ToDto());
        }
    }
}