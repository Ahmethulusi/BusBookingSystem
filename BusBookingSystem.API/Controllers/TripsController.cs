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
        public async Task<IActionResult> SearchTrips(
            [FromQuery] int originId,
            [FromQuery] int? originDistrictId,
             [FromQuery] int destinationId,
             [FromQuery] int? destinationDistrictId,
              [FromQuery] DateTime date)
        {
            try
            {
               var trips = await _tripService.SearchTripsAsync(originId, originDistrictId, destinationId, destinationDistrictId, date.ToString("yyyy-MM-dd"));
            return Ok(Response<IEnumerable<TripDto>>.Successful(trips));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<TripDto>>.Fail(ex.Message));
            }
        }
[HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        try
        {
            var result = await _tripService.DeleteTripAsync(id);
            if (!result)
                return NotFound(Response<bool>.Fail("Sefer bulunamadı"));

            return Ok(Response<bool>.Successful(true, "Sefer başarıyla silindi"));
        }
        catch (InvalidOperationException ex)
        {
            // Bilet satılmışsa 400 hatası ve mesajı döner
            return BadRequest(Response<bool>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(Response<bool>.Fail(ex.Message));
        }
    }
    }
}

