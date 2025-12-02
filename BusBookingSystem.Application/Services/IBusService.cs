// BusBookingSystem.Application/Services/IBusService.cs
using BusBookingSystem.Application.DTOs;

namespace BusBookingSystem.Application.Services
{
    public interface IBusService
    {
        // Geriye hiçbir şey dönmesin (void) veya oluşturulan ID dönsün.
        Task AddBusAsync(CreateBusDto busDto);
        
        // Tüm otobüsleri getir
        Task<IEnumerable<BusDto>> GetAllBusesAsync();
    }
}