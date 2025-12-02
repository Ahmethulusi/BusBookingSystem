using BusBookingSystem.Core.Enums; // Enum'ı kullanmak için

namespace BusBookingSystem.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; }

        public string PassengerName { get; set; }
        public string PassengerTcNo { get; set; }
        public int SeatNumber { get; set; }

        public Gender Gender { get; set; } // Enum kullanımı

        public bool IsReserved { get; set; }
        public bool IsPaid { get; set; }
    }
}