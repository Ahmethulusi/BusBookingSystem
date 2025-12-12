using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Mappers;
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Application.Services.Impl
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        // Süresi geçmiş rezervasyonları temizle
        private async Task CleanExpiredReservationsAsync(int tripId)
        {
            var expiredReservations = await _context.Tickets
                .Where(t => t.TripId == tripId
                    && t.IsReserved
                    && !t.IsPaid
                    && t.ReservationExpiresAt < DateTime.Now)
                .ToListAsync();

            if (expiredReservations.Any())
            {
                _context.Tickets.RemoveRange(expiredReservations);
                await _context.SaveChangesAsync();
            }
        }

        // Koltuk rezerve et
        public async Task<TicketDto> ReserveTicketAsync(int tripId, ReserveTicketDto ticketDto)
        {
            await CleanExpiredReservationsAsync(tripId);

            var trip = await _context.Trips
                .Include(t => t.Bus)
                .Include(t => t.OriginCity)
                .Include(t => t.DestinationCity)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                throw new InvalidOperationException("Sefer bulunamadı");

            if (ticketDto.SeatNumber < 1 || ticketDto.SeatNumber > trip.Bus.TotalSeatCount)
                throw new InvalidOperationException($"Geçersiz koltuk numarası. Bu otobüste 1-{trip.Bus.TotalSeatCount} arası koltuk bulunmaktadır");

            var existingTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TripId == tripId && t.SeatNumber == ticketDto.SeatNumber);

            if (existingTicket != null)
            {
                if (existingTicket.IsPaid)
                    throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk satın alınmış durumda");
                else if (existingTicket.IsReserved)
                    throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk rezerve edilmiş durumda");
            }

            var passenger = await _context.Passengers.FindAsync(ticketDto.PassengerId);
            if (passenger == null)
                throw new InvalidOperationException("Yolcu bulunamadı");

            // Rezervasyon oluştur - 1 saat geçerli
            var ticket = new Ticket
            {
                TripId = tripId,
                PassengerId = ticketDto.PassengerId,
                SeatNumber = ticketDto.SeatNumber,
                PaidAmount = 0,
                IsReserved = true,
                IsPaid = false,
                ReservationExpiresAt = DateTime.Now.AddHours(1),
                CreatedDate = DateTime.Now
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            var createdTicket = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(t => t.Id == ticket.Id);

            return createdTicket!.ToDto();
        }

        // Rezervasyonu tamamla
        public async Task<TicketDto> CompleteReservationAsync(int ticketId, CompleteReservationDto dto)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                throw new InvalidOperationException("Bilet bulunamadı");

            if (!ticket.IsReserved)
                throw new InvalidOperationException("Bu bilet rezerve edilmemiş");

            if (ticket.IsPaid)
                throw new InvalidOperationException("Bu bilet zaten ödenmiş");

            if (ticket.ReservationExpiresAt < DateTime.Now)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                throw new InvalidOperationException("Rezervasyon süresi dolmuş. Lütfen tekrar rezerve edin");
            }

            if (dto.PaidAmount < ticket.Trip.Price)
                throw new InvalidOperationException($"Yetersiz ödeme! Bilet fiyatı: {ticket.Trip.Price} TL, Ödenen tutar: {dto.PaidAmount} TL");

            ticket.PaidAmount = dto.PaidAmount;
            ticket.IsPaid = true;
            ticket.ReservationExpiresAt = null;

            await _context.SaveChangesAsync();

            return ticket.ToDto();
        }

        // Direkt satın alma 
        public async Task<TicketDto> PurchaseTicketAsync(int tripId, CreateTicketDto ticketDto)
        {
            await CleanExpiredReservationsAsync(tripId);

            var trip = await _context.Trips
                .Include(t => t.Bus)
                .Include(t => t.OriginCity)
                .Include(t => t.DestinationCity)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                throw new InvalidOperationException("Sefer bulunamadı");

            if (ticketDto.SeatNumber < 1 || ticketDto.SeatNumber > trip.Bus.TotalSeatCount)
                throw new InvalidOperationException($"Geçersiz koltuk numarası. Bu otobüste 1-{trip.Bus.TotalSeatCount} arası koltuk bulunmaktadır");

            var existingTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TripId == tripId && t.SeatNumber == ticketDto.SeatNumber);

            if (existingTicket != null)
            {
                if (existingTicket.IsPaid)
                    throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk satın alınmış durumda");
                else if (existingTicket.IsReserved)
                    throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk rezerve edilmiş durumda");
            }

            var passenger = await _context.Passengers.FindAsync(ticketDto.PassengerId);
            if (passenger == null)
                throw new InvalidOperationException("Yolcu bulunamadı");

            // Ödeme kontrolü
            if (ticketDto.PaidAmount < trip.Price)
                throw new InvalidOperationException($"Yetersiz ödeme! Bilet fiyatı: {trip.Price} TL, Ödenen tutar: {ticketDto.PaidAmount} TL");

            var ticket = new Ticket
            {
                TripId = tripId,
                PassengerId = ticketDto.PassengerId,
                SeatNumber = ticketDto.SeatNumber,
                PaidAmount = ticketDto.PaidAmount,
                IsReserved = false,
                IsPaid = true,
                ReservationExpiresAt = null,
                CreatedDate = DateTime.Now
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            var createdTicket = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(t => t.Id == ticket.Id);

            return createdTicket!.ToDto();
        }

        public async Task<TripAvailabilityDto> GetTripAvailabilityAsync(int tripId)
        {
            await CleanExpiredReservationsAsync(tripId);

            var trip = await _context.Trips
                .Include(t => t.Bus)
                .Include(t => t.Tickets)
                    .ThenInclude(ticket => ticket.Passenger)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                throw new InvalidOperationException("Sefer bulunamadı");

            var totalSeats = trip.Bus.TotalSeatCount;
            var occupiedSeats = trip.Tickets.Count;
            var availableSeats = totalSeats - occupiedSeats;

            var seats = new List<SeatAvailabilityDto>();

            // Tüm koltukları listele
            for (int seatNumber = 1; seatNumber <= totalSeats; seatNumber++)
            {
                var ticket = trip.Tickets.FirstOrDefault(t => t.SeatNumber == seatNumber);

                string seatStatus = "Available";
                if (ticket != null)
                {
                    if (ticket.IsPaid)
                        seatStatus = "Sold";
                    else if (ticket.IsReserved)
                        seatStatus = "Reserved";
                }

                seats.Add(new SeatAvailabilityDto
                {
                    SeatNumber = seatNumber,
                    IsAvailable = ticket == null,
                    PassengerName = ticket?.IsPaid == true ? $"{ticket.Passenger.FirstName} {ticket.Passenger.LastName}" : null,
                    Status = seatStatus,
                    ReservationExpiresAt = ticket?.IsReserved == true ? ticket.ReservationExpiresAt : null
                });
            }

            return new TripAvailabilityDto
            {
                TripId = tripId,
                TotalSeats = totalSeats,
                AvailableSeats = availableSeats,
                OccupiedSeats = occupiedSeats,
                Seats = seats
            };
        }

        public async Task<bool> IsSeatAvailableAsync(int tripId, int seatNumber)
        {
            await CleanExpiredReservationsAsync(tripId);

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TripId == tripId && t.SeatNumber == seatNumber);

            return ticket == null;
        }

        // Yolcunun biletlerini getir
        public async Task<IEnumerable<TicketDto>> GetPassengerTicketsAsync(int passengerId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Passenger)
                .Where(t => t.PassengerId == passengerId)
                .ToListAsync();

            return tickets.ToDto();
        }

        // Sefer biletlerini getir
        public async Task<IEnumerable<TicketDto>> GetTripTicketsAsync(int tripId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Passenger)
                .Where(t => t.TripId == tripId)
                .OrderBy(t => t.SeatNumber)
                .ToListAsync();

            return tickets.ToDto();
        }

        //Bileti iptal et
        public async Task<bool> CancelTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket == null)
                return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }

        //Id'ye göre bilet detayı getirme
        public async Task<TicketDto?> GetTicketByIdAsync(int ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            return ticket?.ToDto();
        }
    }
    
}
