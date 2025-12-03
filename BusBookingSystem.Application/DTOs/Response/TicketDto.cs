namespace BusBookingSystem.Application.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int PassengerId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsReserved { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? ReservationExpiresAt { get; set; }
        public DateTime CreatedDate { get; set; }

        // Yolculuk bilgileri
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public decimal TripPrice { get; set; }
    }
}