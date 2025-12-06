// BusBookingSystem.Application/DTOs/Response/DistrictDto.cs
namespace BusBookingSystem.Application.DTOs.Response
{
    public class DistrictDto
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

