// BusBookingSystem.Application/Services/IBusService.cs
using BusBookingSystem.Application.DTOs;

namespace BusBookingSystem.Application.Services
{
    public interface IBusService
    {
        // Geriye hiçbir şey dönmesin (void) veya oluşturulan ID dönsün.
        Task AddBusAsync(CreateBusDto busDto);
        
        // Şimdilik sadece ekleme yapalım, listelemeyi sonra ekleriz.
    }
}