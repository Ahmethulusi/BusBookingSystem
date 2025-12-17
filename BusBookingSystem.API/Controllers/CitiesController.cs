// BusBookingSystem.API/Controllers/CitiesController.cs
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            try
            {
                var cities = await _locationService.GetAllCitiesAsync();
                return Ok(Response<IEnumerable<CityDto>>.Successful(cities));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<CityDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("{cityId}/districts")]
        public async Task<IActionResult> GetDistrictsByCityId(int cityId)
        {
            try
            {
                var districts = await _locationService.GetDistrictsByCityIdAsync(cityId);
                return Ok(Response<IEnumerable<DistrictDto>>.Successful(districts));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(Response<IEnumerable<DistrictDto>>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<DistrictDto>>.Fail(ex.Message));
            }
        }
    }
}

