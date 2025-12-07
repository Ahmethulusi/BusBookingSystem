// BusBookingSystem.API/Controllers/BusesController.cs
using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BusesController : ControllerBase
    {
        private readonly IBusService _busService;

        public BusesController(IBusService busService)
        {
            _busService = busService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBuses()
        {
            try
            {
                var buses = await _busService.GetAllBusesAsync();
                return Ok(Response<IEnumerable<BusDto>>.Successful(buses));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<BusDto>>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBus([FromBody] CreateBusDto request)
        {
            try
            {
                await _busService.AddBusAsync(request);
                return Ok(Response<bool>.Successful(true));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<bool>.Fail(ex.Message));
            }
        }
    }
}