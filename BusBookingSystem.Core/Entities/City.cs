namespace BusBookingSystem.Core.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // Navigation Property - Bir şehirde birden fazla ilçe olabilir
        public ICollection<District> Districts { get; set; } = new List<District>();
    }
}

