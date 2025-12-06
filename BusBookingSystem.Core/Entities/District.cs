namespace BusBookingSystem.Core.Entities
{
    public class District : BaseEntity
    {
        public int CityId { get; set; }
        public City City { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
    }
}

