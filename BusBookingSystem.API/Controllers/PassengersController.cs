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
            try
            {
                var passengers = await _passengerService.GetAllPassengersAsync();
                return Ok(Response<IEnumerable<PassengerDto>>.Successful(passengers));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<PassengerDto>>.Fail(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPassengerById(int id)
        {
            try
            {
                var passenger = await _passengerService.GetPassengerByIdAsync(id);
                if (passenger == null)
                    return NotFound(Response<PassengerDto>.Fail("Yolcu bulunamadı"));

                return Ok(Response<PassengerDto>.Successful(passenger));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<PassengerDto>.Fail(ex.Message));
            }
        }

        [HttpGet("tc/{tcNo}")]
        public async Task<IActionResult> GetPassengerByTcNo(string tcNo)
        {
            try
            {
                var passenger = await _passengerService.GetPassengerByTcNoAsync(tcNo);
                if (passenger == null)
                    return NotFound(Response<PassengerDto>.Fail("Belirtilen TC kimlik numarası ile yolcu bulunamadı"));

                return Ok(Response<PassengerDto>.Successful(passenger));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<PassengerDto>.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePassenger([FromBody] CreatePassengerDto request)
        {
            try
            {
                var passenger = await _passengerService.AddPassengerAsync(request);
                return Ok(Response<PassengerDto>.Successful(passenger, "Yolcu başarıyla oluşturulmuştur"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(Response<PassengerDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<PassengerDto>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassenger(int id, [FromBody] UpdatePassengerDto request)
        {
            try
            {
                var updatedPassenger = await _passengerService.UpdatePassengerAsync(id, request);
                if (updatedPassenger == null)
                    return NotFound(Response<PassengerDto>.Fail("Yolcu bulunamadı"));

                return Ok(Response<PassengerDto>.Successful(updatedPassenger, "Yolcu bilgileri başarıyla güncellenmiştir"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(Response<PassengerDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<PassengerDto>.Fail(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            try
            {
                var deleted = await _passengerService.DeletePassengerAsync(id);
                if (!deleted)
                    return NotFound(Response<bool>.Fail("Yolcu bulunamadı"));

                return Ok(Response<bool>.Successful(true, $"{id} ID'li yolcu başarıyla silinmiştir"));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<bool>.Fail(ex.Message));
            }
        }

        [HttpGet("exists/tc/{tcNo}")]
        public async Task<IActionResult> CheckPassengerExistsByTcNo(string tcNo)
        {
            try
            {
                var exists = await _passengerService.PassengerExistsByTcNoAsync(tcNo);
                return Ok(Response<bool>.Successful(exists));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<bool>.Fail(ex.Message));
            }
        }

        [HttpGet("exists/email/{email}")]
        public async Task<IActionResult> CheckPassengerExistsByEmail(string email)
        {
            try
            {
                var exists = await _passengerService.PassengerExistsByEmailAsync(email);
                return Ok(Response<bool>.Successful(exists));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<bool>.Fail(ex.Message));
            }
        }
    }
}