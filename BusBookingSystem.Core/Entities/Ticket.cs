using BusBookingSystem.Core.Enums;

namespace BusBookingSystem.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; } = null!;

        public int SeatNumber { get; set; }
        public decimal PaidAmount { get; set; }

        public bool IsReserved { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? ReservationExpiresAt { get; set; }
    }
}