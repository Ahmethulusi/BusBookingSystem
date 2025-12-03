// BusBookingSystem.Application/Services/ITripService.cs
using BusBookingSystem.Application.DTOs;

namespace BusBookingSystem.Application.Services
{
    public interface ITripService
    {
        Task AddTripAsync(CreateTripDto tripDto);
        Task<IEnumerable<TripDto>> GetAllTripsAsync();
    }
}

