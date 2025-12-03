namespace BusBookingSystem.Core.Entities
{
    public class Bus : BaseEntity
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int TotalSeatCount { get; set; }

        // Navigation Property
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}