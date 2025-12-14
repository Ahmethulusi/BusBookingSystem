using BusBookingSystem.Application.DTOs.Response;

namespace BusBookingSystem.Application.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        
        public int SeatNumber { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsReserved { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? ReservationExpiresAt { get; set; }
        public DateTime CreatedDate { get; set; }

        // Yolculuk bilgileri
        public TripDto? Trip { get; set; }       
        public PassengerDto? Passenger { get; set; }
    }
}