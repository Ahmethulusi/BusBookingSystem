namespace BusBookingSystem.Core.Entities
{
    public class Trip : BaseEntity
    {
        public int BusId { get; set; }
        public Bus Bus { get; set; } = null!;

        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public decimal Price { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}