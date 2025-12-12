// BusBookingSystem.API/Controllers/TripsController.cs
using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrips()
        {
            try
            {
                var trips = await _tripService.GetAllTripsAsync();
                return Ok(Response<IEnumerable<TripDto>>.Successful(trips));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<TripDto>>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto request)
        {
            try
            {
                var createdTrip = await _tripService.AddTripAsync(request);
                return Ok(Response<TripDto>.Successful(createdTrip, "Sefer başarıyla oluşturulmuştur"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Response<TripDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<TripDto>.Fail(ex.Message));
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchTrips([FromQuery] int originId, [FromQuery] int destinationId, [FromQuery] DateTime date)    
        {
         var trips = await _tripService.SearchTripsAsync(originId, destinationId, date);

        return Ok(new { success = true, body = trips }); 
}
    }
}

