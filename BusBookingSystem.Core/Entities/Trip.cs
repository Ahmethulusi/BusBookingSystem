namespace BusBookingSystem.Core.Entities
{
    public class Trip : BaseEntity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public int BusId { get; set; }
        public Bus Bus { get; set; } = null!;

        // Origin (Kalkış) bilgileri
        public int OriginCityId { get; set; }
        public City OriginCity { get; set; } = null!;
        public int? OriginDistrictId { get; set; } // Nullable - İlçe opsiyonel
        public District? OriginDistrict { get; set; }

        // Destination (Varış) bilgileri
        public int DestinationCityId { get; set; }
        public City DestinationCity { get; set; } = null!;
        public int? DestinationDistrictId { get; set; } // Nullable - İlçe opsiyonel
        public District? DestinationDistrict { get; set; }

        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public decimal Price { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}