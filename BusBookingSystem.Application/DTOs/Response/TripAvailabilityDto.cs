namespace BusBookingSystem.Application.DTOs.Response
{
    public class TripAvailabilityDto
    {
        public int TripId { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int OccupiedSeats { get; set; }
        public List<SeatAvailabilityDto> Seats { get; set; } = new List<SeatAvailabilityDto>();
    }
}
