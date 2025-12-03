// BusBookingSystem.Application/DTOs/TripDto.cs
namespace BusBookingSystem.Application.DTOs
{
    public class TripDto
    {
        public int Id { get; set; }
        public int BusId { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

