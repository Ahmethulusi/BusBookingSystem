namespace BusBookingSystem.Application.DTOs.Response
{
    public class SeatAvailabilityDto
    {
        public int SeatNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string? PassengerName { get; set; }
        public string Status { get; set; } = "Available"; // "Available", "Reserved", "Sold"
        public DateTime? ReservationExpiresAt { get; set; }

        public int Gender {get; set;}
    }
}
