using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;

namespace BusBookingSystem.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<List<UserDto>> GetAllUsersAsync();
    }
}
