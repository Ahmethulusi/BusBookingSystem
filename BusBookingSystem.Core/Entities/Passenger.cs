using BusBookingSystem.Core.Enums;

namespace BusBookingSystem.Core.Entities
{
    public class Passenger : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string TcNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Navigation Properties
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}