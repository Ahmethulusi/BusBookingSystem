// BusBookingSystem.API/Controllers/CitiesController.cs
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public CitiesController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _locationService.GetAllCitiesAsync();
            return Ok(cities);
        }

        [HttpGet("{cityId}/districts")]
        public async Task<IActionResult> GetDistrictsByCityId(int cityId)
        {
            try
            {
                var districts = await _locationService.GetDistrictsByCityIdAsync(cityId);
                return Ok(districts);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "İlçeler getirilirken bir hata oluştu.", error = ex.Message });
            }
        }
    }
}

