// BusBookingSystem.Application/Services/ILocationService.cs
using BusBookingSystem.Application.DTOs.Response;

namespace BusBookingSystem.Application.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<CityDto>> GetAllCitiesAsync();
        Task<IEnumerable<DistrictDto>> GetDistrictsByCityIdAsync(int cityId);
    }
}

