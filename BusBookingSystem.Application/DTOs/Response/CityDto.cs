// BusBookingSystem.Application/DTOs/Response/CityDto.cs
namespace BusBookingSystem.Application.DTOs.Response
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<DistrictDto> Districts { get; set; } = new List<DistrictDto>();
    }
}

