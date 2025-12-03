// BusBookingSystem.Application/DTOs/CreateBusDto.cs
namespace BusBookingSystem.Application.DTOs
{
    // Frontend'den otobüs eklerken sadece bu bilgileri istiyoruz.
    // ID veya CreatedDate istemiyoruz, onları biz yöneteceğiz.
    public class CreateBusDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int TotalSeatCount { get; set; }
    }
}