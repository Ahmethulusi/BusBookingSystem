// BusBookingSystem.Application/Services/IBusService.cs
using BusBookingSystem.Application.DTOs;

namespace BusBookingSystem.Application.Services
{
    public interface IBusService
    {
        Task<BusDto> AddBusAsync(CreateBusDto busDto);
        Task<IEnumerable<BusDto>> GetAllBusesAsync();
    }
}