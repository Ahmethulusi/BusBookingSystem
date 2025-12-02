// BusBookingSystem.API/Controllers/TripsController.cs
using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
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
            // Servisten tüm seferleri al
            var trips = await _tripService.GetAllTripsAsync();

            // Başarılı (200 OK) ile listeyi döndür
            return Ok(trips);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripDto request)
        {
            try
            {
                // Servise işi devret
                await _tripService.AddTripAsync(request);

                // Başarılı (200 OK) dön
                return Ok(new { message = "Sefer başarıyla oluşturuldu!" });
            }
            catch (ArgumentException ex)
            {
                // Bus bulunamadı hatası
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Diğer hatalar
                return StatusCode(500, new { message = "Sefer oluşturulurken bir hata oluştu.", error = ex.Message });
            }
        }
    }
}

