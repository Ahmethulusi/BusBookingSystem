using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var response = await _authService.RegisterAsync(registerDto);
                return Ok(Response<AuthResponseDto>.Successful(response));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(Response<AuthResponseDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<AuthResponseDto>.Fail(ex.Message));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = await _authService.LoginAsync(loginDto);
                return Ok(Response<AuthResponseDto>.Successful(response));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(Response<AuthResponseDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<AuthResponseDto>.Fail(ex.Message));
            }
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();
                return Ok(Response<IEnumerable<UserDto>>.Successful(users));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<UserDto>>.Fail(ex.Message));
            }
        }
    }
}
