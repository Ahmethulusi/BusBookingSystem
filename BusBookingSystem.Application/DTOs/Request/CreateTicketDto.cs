namespace BusBookingSystem.Application.DTOs
{
    // Rezervasyon için kullanılacak
    public class ReserveTicketDto
    {
        public int PassengerId { get; set; }
        public int SeatNumber { get; set; }
    }

    // Direkt satın alma için kullanılacak (eski CreateTicketDto)
    public class CreateTicketDto
    {
        public int PassengerId { get; set; }
        public int SeatNumber { get; set; }
    }

    // Rezervasyonu tamamlama için kullanılacak
    public class CompleteReservationDto
    {
        public decimal PaidAmount { get; set; }
    }
}