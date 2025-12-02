namespace BusBookingSystem.Core.Entities
{
    public class Bus : BaseEntity
    {
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public int TotalSeatCount { get; set; }

        // Navigation Property
        public ICollection<Trip> Trips { get; set; }
    }
}