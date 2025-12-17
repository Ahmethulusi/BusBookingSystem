namespace BusBookingSystem.Application.DTOs
{
    // Rezervasyon için 
    public class ReserveTicketDto
    {
        public int PassengerId { get; set; }
        public int SeatNumber { get; set; }
    }

    // Direkt satın alma 
    public class CreateTicketDto
    {
        public int PassengerId { get; set; }
        public int SeatNumber { get; set; }
        public decimal PaidAmount { get; set; }
    }

    // Rezervasyonu tamamlama için 
    public class CompleteReservationDto
    {
        public decimal PaidAmount { get; set; }
    }
}