// BusBookingSystem.Application/DTOs/Response/TripDto.cs
namespace BusBookingSystem.Application.DTOs.Response
{
    public class TripDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int BusId { get; set; }

        // Origin (Kalkış) bilgileri
        public int OriginCityId { get; set; }
        public string OriginCityName { get; set; } = string.Empty;
        public int? OriginDistrictId { get; set; }
        public string? OriginDistrictName { get; set; }

        // Destination (Varış) bilgileri
        public int DestinationCityId { get; set; }
        public string DestinationCityName { get; set; } = string.Empty;
        public int? DestinationDistrictId { get; set; }
        public string? DestinationDistrictName { get; set; }

        public string DepartureDate { get; set; } = string.Empty;
        public string DepartureTime { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? CompanyName { get; set; }

        public int SoldTicketCount { get; set; }

        public string BusPlateNumber { get; set; } = string.Empty;
    }
}

