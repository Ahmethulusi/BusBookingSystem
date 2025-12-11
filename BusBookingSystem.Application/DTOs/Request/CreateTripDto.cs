// BusBookingSystem.Application/DTOs/Request/CreateTripDto.cs
namespace BusBookingSystem.Application.DTOs.Request
{
    public class CreateTripDto
    {
        public int CompanyId { get; set; }
        public int BusId { get; set; }

        // Origin (Kalkış) bilgileri
        public int OriginCityId { get; set; }
        public int? OriginDistrictId { get; set; } // Nullable - İlçe opsiyonel

        // Destination (Varış) bilgileri
        public int DestinationCityId { get; set; }
        public int? DestinationDistrictId { get; set; } // Nullable - İlçe opsiyonel

        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public decimal Price { get; set; }
    }
}

