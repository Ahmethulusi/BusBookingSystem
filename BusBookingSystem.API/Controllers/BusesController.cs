// BusBookingSystem.API/Controllers/BusesController.cs
using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly IBusService _busService;

        // Servisi enjekte ediyoruz
        public BusesController(IBusService busService)
        {
            _busService = busService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBus([FromBody] CreateBusDto request)
        {
            // Servise işi devret
            await _busService.AddBusAsync(request);
            
            // Başarılı (200 OK) dön
            return Ok(new { message = "Otobüs başarıyla eklendi!" });
        }
    }
}