// BusBookingSystem.Application/Services/ITripService.cs
using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;

namespace BusBookingSystem.Application.Services
{
    public interface ITripService
    {
        Task<TripDto> AddTripAsync(CreateTripDto tripDto);
        Task<IEnumerable<TripDto>> GetAllTripsAsync();

        Task<IEnumerable<TripDto>> SearchTripsAsync(int originId, int destinationId, string date);
        Task<bool> DeleteTripAsync(int id);
    }
}

