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

        public async Task<TicketDto> PurchaseTicketAsync(int tripId, CreateTicketDto ticketDto)
        {
            // 1. Seferi kontrol et
            var trip = await _context.Trips
                .Include(t => t.Bus)
                .Include(t => t.OriginCity)
                .Include(t => t.DestinationCity)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                throw new InvalidOperationException("Sefer bulunamadı");

            if (ticketDto.SeatNumber < 1 || ticketDto.SeatNumber > trip.Bus.TotalSeatCount)
                throw new InvalidOperationException($"Geçersiz koltuk numarası. Bu otobüste 1-{trip.Bus.TotalSeatCount} arası koltuk bulunmaktadır");

            var seatTaken = await _context.Tickets
                .AnyAsync(t => t.TripId == tripId && t.SeatNumber == ticketDto.SeatNumber);

            if (seatTaken)
                throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk zaten dolu");

            var passenger = await _context.Passengers.FindAsync(ticketDto.PassengerId);
            if (passenger == null)
                throw new InvalidOperationException("Yolcu bulunamadı");

            if (ticketDto.PaidAmount > 0 && ticketDto.PaidAmount < trip.Price)
                throw new InvalidOperationException($"Yetersiz ödeme! Bilet fiyatı: {trip.Price} TL, Ödenen tutar: {ticketDto.PaidAmount} TL");

            var ticket = new Ticket
            {
                TripId = tripId,
                PassengerId = ticketDto.PassengerId,
                SeatNumber = ticketDto.SeatNumber,
                PaidAmount = ticketDto.PaidAmount > 0 ? ticketDto.PaidAmount : trip.Price,
                IsReserved = ticketDto.IsReserved,
                IsPaid = ticketDto.IsPaid,
                ReservationExpiresAt = ticketDto.ReservationExpiresAt,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            // Include ilişkili verileri yükle ve DTO'ya çevir
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

                seats.Add(new SeatAvailabilityDto
                {
                    SeatNumber = seatNumber,
                    IsAvailable = ticket == null,
                    PassengerName = ticket != null ? $"{ticket.Passenger.FirstName} {ticket.Passenger.LastName}" : null
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
            var isTaken = await _context.Tickets
                .AnyAsync(t => t.TripId == tripId && t.SeatNumber == seatNumber);

            return !isTaken;
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
