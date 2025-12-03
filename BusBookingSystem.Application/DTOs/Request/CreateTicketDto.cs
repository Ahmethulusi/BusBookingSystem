namespace BusBookingSystem.Application.DTOs
{
    public class CreateTicketDto
    {
        public int PassengerId { get; set; }
        public int SeatNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsReserved { get; set; } = true;
        public bool IsPaid { get; set; } = false;
        public DateTime? ReservationExpiresAt { get; set; }
    }
}