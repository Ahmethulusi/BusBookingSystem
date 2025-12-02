// BusBookingSystem.Application/DTOs/CreateTripDto.cs
namespace BusBookingSystem.Application.DTOs
{
    public class CreateTripDto
    {
        public int BusId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal Price { get; set; }
    }
}

