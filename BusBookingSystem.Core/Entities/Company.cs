namespace BusBookingSystem.Core.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        // Navigation Properties
        public ICollection<Bus> Buses { get; set; } = new List<Bus>();
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}

