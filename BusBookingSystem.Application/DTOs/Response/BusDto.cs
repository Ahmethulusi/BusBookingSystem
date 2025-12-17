// BusBookingSystem.Application/DTOs/BusDto.cs
namespace BusBookingSystem.Application.DTOs
{
    public class BusDto
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int TotalSeatCount { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CompanyName { get; set; }
    }
}

