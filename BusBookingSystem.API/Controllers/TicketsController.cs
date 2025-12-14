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
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        // Bilet satın al
        [HttpPost("trips/{tripId}/purchase")]
        public async Task<IActionResult> PurchaseTicket(int tripId, [FromBody] CreateTicketDto ticketDto)
        {
            try
            {
                var ticket = await _ticketService.PurchaseTicketAsync(tripId, ticketDto);
                return Ok(Response<TicketDto>.Successful(ticket, "Biletiniz başarıyla satın alınmıştır"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(Response<TicketDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<TicketDto>.Fail(ex.Message));
            }
        }

        // Koltuk rezerve et
        [HttpPost("trips/{tripId}/reserve")]
        public async Task<IActionResult> ReserveTicket(int tripId, [FromBody] ReserveTicketDto ticketDto)
        {
            try
            {
                var ticket = await _ticketService.ReserveTicketAsync(tripId, ticketDto);
                return Ok(Response<TicketDto>.Successful(ticket, $"Koltuk rezerve edildi. {ticket.ReservationExpiresAt:dd.MM.yyyy HH:mm} tarihine kadar geçerlidir"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(Response<TicketDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<TicketDto>.Fail(ex.Message));
            }
        }

        // Rezervasyonu tamamla
        [HttpPost("{ticketId}/complete-reservation")]
        public async Task<IActionResult> CompleteReservation(int ticketId, [FromBody] CompleteReservationDto dto)
        {
            try
            {
                var ticket = await _ticketService.CompleteReservationAsync(ticketId, dto);
                return Ok(Response<TicketDto>.Successful(ticket, "Rezervasyonunuz tamamlandı. Biletiniz satın alınmıştır"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(Response<TicketDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<TicketDto>.Fail(ex.Message));
            }
        }

        // Sefer için koltuk durumunu görüntüle
        [HttpGet("trips/{tripId}/availability")]
        public async Task<IActionResult> GetTripAvailability(int tripId)
        {
            try
            {
                var availability = await _ticketService.GetTripAvailabilityAsync(tripId);
                return Ok(Response<TripAvailabilityDto>.Successful(availability));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(Response<TripAvailabilityDto>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<TripAvailabilityDto>.Fail(ex.Message));
            }
        }

        // Belirli bir koltuğun uygunluğunu kontrol et
        [HttpGet("trips/{tripId}/seats/{seatNumber}/available")]
        public async Task<IActionResult> IsSeatAvailable(int tripId, int seatNumber)
        {
            try
            {
                var isAvailable = await _ticketService.IsSeatAvailableAsync(tripId, seatNumber);
                return Ok(Response<bool>.Successful(isAvailable));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<bool>.Fail(ex.Message));
            }
        }

        //Yolcunun tüm biletlerini getir
        [HttpGet("passengers/{passengerId}")]
        public async Task<IActionResult> GetPassengerTickets(int passengerId)
        {
            try
            {
                var tickets = await _ticketService.GetPassengerTicketsAsync(passengerId);
                return Ok(Response<IEnumerable<TicketDto>>.Successful(tickets));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<TicketDto>>.Fail(ex.Message));
            }
        }

        // Sefer için satılan tüm biletleri getir
        [HttpGet("trips/{tripId}")]
        public async Task<IActionResult> GetTripTickets(int tripId)
        {
            try
            {
                var tickets = await _ticketService.GetTripTicketsAsync(tripId);
                return Ok(Response<IEnumerable<TicketDto>>.Successful(tickets));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<IEnumerable<TicketDto>>.Fail(ex.Message));
            }
        }

        //Id'ye göre bilet detayını getir
        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetTicketById(int ticketId)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(ticketId);
                if (ticket == null)
                    return NotFound(Response<TicketDto>.Fail("Bilet bulunamadı"));

                return Ok(Response<TicketDto>.Successful(ticket));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<TicketDto>.Fail(ex.Message));
            }
        }

        // Bilet iptal et
        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> CancelTicket(int ticketId)
        {
            try
            {
                var result = await _ticketService.CancelTicketAsync(ticketId);
                if (!result)
                    return NotFound(Response<bool>.Fail("Bilet bulunamadı"));

                return Ok(Response<bool>.Successful(true, $"{ticketId} ID'li biletiniz başarıyla iptal edilmiştir"));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<bool>.Fail(ex.Message));
            }
        }
        [HttpGet("trips/{tripId}/seats/{seatNumber}/validate-gender")]
        public async Task<IActionResult> ValidateSeatGender(int tripId, int seatNumber, [FromQuery] int gender)
        {
            try
            {
                await _ticketService.ValidateSeatGenderAsync(tripId, seatNumber, gender);
                return Ok(Response<bool>.Successful(true, "Koltuk uygun"));
            }
            catch (InvalidOperationException ex)
            {
                // Kural hatası varsa 400 dön ve mesajı gönder
                return BadRequest(Response<bool>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<bool>.Fail(ex.Message));
            }
        }
    }
}
