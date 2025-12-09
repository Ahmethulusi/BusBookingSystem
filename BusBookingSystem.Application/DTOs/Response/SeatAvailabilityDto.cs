namespace BusBookingSystem.Application.DTOs.Response
{
    public class SeatAvailabilityDto
    {
        public int SeatNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string? PassengerName { get; set; }
    }
}
