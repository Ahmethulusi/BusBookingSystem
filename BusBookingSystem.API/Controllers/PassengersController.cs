using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly IPassengerService _passengerService;

        public PassengersController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPassengers()
        {
            var passengers = await _passengerService.GetAllPassengersAsync();
            return Ok(passengers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPassengerById(int id)
        {
            var passenger = await _passengerService.GetPassengerByIdAsync(id);
            if (passenger == null)
                return NotFound(new { message = "Yolcu bulunamadı" });

            return Ok(passenger);
        }

        [HttpGet("tc/{tcNo}")]
        public async Task<IActionResult> GetPassengerByTcNo(string tcNo)
        {
            var passenger = await _passengerService.GetPassengerByTcNoAsync(tcNo);
            if (passenger == null)
                return NotFound(new { message = "Belirtilen TC kimlik numarası ile yolcu bulunamadı" });

            return Ok(passenger);
        }



        [HttpPost]
        public async Task<IActionResult> CreatePassenger([FromBody] CreatePassengerDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var passenger = await _passengerService.AddPassengerAsync(request);
                return CreatedAtAction(nameof(GetPassengerById), new { id = passenger.Id }, passenger);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Yolcu oluşturulurken bir hata oluştu.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassenger(int id, [FromBody] UpdatePassengerDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedPassenger = await _passengerService.UpdatePassengerAsync(id, request);
                if (updatedPassenger == null)
                    return NotFound(new { message = "Yolcu bulunamadı" });

                return Ok(updatedPassenger);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Yolcu güncellenirken bir hata oluştu.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            try
            {
                var deleted = await _passengerService.DeletePassengerAsync(id);
                if (!deleted)
                    return NotFound(new { message = "Yolcu bulunamadı" });

                return Ok(new { message = "Yolcu başarıyla silindi" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Yolcu silinirken bir hata oluştu.", error = ex.Message });
            }
        }

        [HttpGet("exists/tc/{tcNo}")]
        public async Task<IActionResult> CheckPassengerExistsByTcNo(string tcNo)
        {
            var exists = await _passengerService.PassengerExistsByTcNoAsync(tcNo);
            return Ok(new { exists });
        }

        [HttpGet("exists/email/{email}")]
        public async Task<IActionResult> CheckPassengerExistsByEmail(string email)
        {
            var exists = await _passengerService.PassengerExistsByEmailAsync(email);
            return Ok(new { exists });
        }


    }
}