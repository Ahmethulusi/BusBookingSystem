using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.DTOs.Response;

namespace BusBookingSystem.Application.Services
{
    public interface ITicketService
    {
        Task<TicketDto> PurchaseTicketAsync(int tripId, CreateTicketDto ticketDto);

        Task<TripAvailabilityDto> GetTripAvailabilityAsync(int tripId);

        Task<bool> IsSeatAvailableAsync(int tripId, int seatNumber);

        Task<IEnumerable<TicketDto>> GetPassengerTicketsAsync(int passengerId);

        Task<IEnumerable<TicketDto>> GetTripTicketsAsync(int tripId);

        Task<bool> CancelTicketAsync(int ticketId);

        Task<TicketDto?> GetTicketByIdAsync(int ticketId);
    }
}
