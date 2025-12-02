// BusBookingSystem.Application/DTOs/BusDto.cs
namespace BusBookingSystem.Application.DTOs
{
    public class BusDto
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public int TotalSeatCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

