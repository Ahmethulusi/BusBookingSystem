// BusBookingSystem.Application/DTOs/CreateBusDto.cs
namespace BusBookingSystem.Application.DTOs
{
    public class CreateBusDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int TotalSeatCount { get; set; }
        public int CompanyId { get; set; }
    }
}