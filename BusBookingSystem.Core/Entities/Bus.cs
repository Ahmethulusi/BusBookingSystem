namespace BusBookingSystem.Core.Entities
{
    public class Bus : BaseEntity
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int TotalSeatCount { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
    }
}