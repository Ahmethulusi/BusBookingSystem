// BusBookingSystem.Application/DTOs/TripDto.cs
namespace BusBookingSystem.Application.DTOs
{
    public class TripDto
    {
        public int Id { get; set; }
        public int BusId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

