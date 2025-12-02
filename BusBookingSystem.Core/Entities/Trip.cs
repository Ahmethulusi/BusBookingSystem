namespace BusBookingSystem.Core.Entities
{
    public class Trip : BaseEntity
    {
        public int BusId { get; set; }
        public Bus Bus { get; set; } // İlişki

        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal Price { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}