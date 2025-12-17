using BusBookingSystem.Application.DTOs;

namespace BusBookingSystem.Application.Services
{
    public interface IPassengerService
    {
        Task<PassengerDto> AddPassengerAsync(CreatePassengerDto passengerDto);
        Task<IEnumerable<PassengerDto>> GetAllPassengersAsync();
        Task<PassengerDto?> GetPassengerByIdAsync(int id);
        Task<PassengerDto?> GetPassengerByTcNoAsync(string tcNo);
        Task<PassengerDto?> UpdatePassengerAsync(int id, UpdatePassengerDto updateDto);
        Task<bool> DeletePassengerAsync(int id);
        Task<bool> PassengerExistsByTcNoAsync(string tcNo);
        Task<bool> PassengerExistsByEmailAsync(string email);
    }
}