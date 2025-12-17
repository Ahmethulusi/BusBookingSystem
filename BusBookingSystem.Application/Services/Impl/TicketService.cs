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
                {
                    throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk satın alınmış durumda");
                }
                else if (existingTicket.IsReserved)
                {
                    if (existingTicket.ReservationExpiresAt > DateTime.Now)
                    {
                        throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk rezerve edilmiş durumda");
                    }
                    else
                    {
                        // Süresi dolan rezervesyonu sil
                        _context.Tickets.Remove(existingTicket);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            var passenger = await _context.Passengers.FindAsync(ticketDto.PassengerId);
            if (passenger == null)
                throw new InvalidOperationException("Yolcu bulunamadı");

            // Aynı yolcu bu seferde zaten bilet almış mı kontrol et
            var passengerExistingTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TripId == tripId && t.PassengerId == ticketDto.PassengerId);

            if (passengerExistingTicket != null)
            {
                // Eğer expire olmuş bir rezervasyon ise, onu sil ve devam et
                if (passengerExistingTicket.IsReserved && !passengerExistingTicket.IsPaid &&
                    passengerExistingTicket.ReservationExpiresAt < DateTime.Now)
                {
                    _context.Tickets.Remove(passengerExistingTicket);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Bu yolcu bu sefer için zaten bir bilete sahip. Aynı yolcu aynı seferde birden fazla bilet alamaz.");
                }
            }

            // Cinsiyet Kuralı 
            await CheckGenderRuleAsync(tripId, ticketDto.SeatNumber, (int)passenger.Gender, passenger.Id);

            // Yeni Rezervasyon Oluştur (5 Dakika Süreli)
            var ticket = new Ticket
            {
                TripId = tripId,
                PassengerId = ticketDto.PassengerId,
                SeatNumber = ticketDto.SeatNumber,
                PaidAmount = 0,
                IsReserved = true,
                IsPaid = false,
                ReservationExpiresAt = DateTime.Now.AddMinutes(5),
                CreatedDate = DateTime.Now
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            var createdTicket = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Bus)
                        .ThenInclude(b => b.Company)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Tickets)
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
                    .ThenInclude(tr => tr.OriginDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Bus)
                        .ThenInclude(b => b.Company)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Tickets)
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
                {
                    throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk satın alınmış durumda");
                }
                else if (existingTicket.IsReserved)
                {
                    if (existingTicket.ReservationExpiresAt > DateTime.Now)
                    {
                        throw new InvalidOperationException($"{ticketDto.SeatNumber} numaralı koltuk rezerve edilmiş durumda");
                    }
                    else
                    {
                        // Süresi dolmuş rezervasyonu SİL ve SATIN ALMAYA İZİN VER
                        _context.Tickets.Remove(existingTicket);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            var passenger = await _context.Passengers.FindAsync(ticketDto.PassengerId);
            if (passenger == null)
                throw new InvalidOperationException("Yolcu bulunamadı");

            // Aynı yolcu bu seferde zaten bilet almış mı kontrol et
            var passengerExistingTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TripId == tripId && t.PassengerId == ticketDto.PassengerId);

            if (passengerExistingTicket != null)
            {
                if (passengerExistingTicket.IsReserved && !passengerExistingTicket.IsPaid &&
                    passengerExistingTicket.ReservationExpiresAt < DateTime.Now)
                {
                    _context.Tickets.Remove(passengerExistingTicket);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Bu yolcu bu sefer için zaten bir bilete sahip. Aynı yolcu aynı seferde birden fazla bilet alamaz.");
                }
            }

            // Cinsiyet Kuralı
            await CheckGenderRuleAsync(tripId, ticketDto.SeatNumber, (int)passenger.Gender, passenger.Id);

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
                    .ThenInclude(tr => tr.OriginDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Bus)
                        .ThenInclude(b => b.Company)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Tickets)
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
            var seats = new List<SeatAvailabilityDto>();

            // Tüm koltukları listele
            for (int seatNumber = 1; seatNumber <= totalSeats; seatNumber++)
            {
                var ticket = trip.Tickets.FirstOrDefault(t => t.SeatNumber == seatNumber);

                // Eğer bilet var Ama Rezerve ve süresi dolmuşsa onu yok say.
                if (ticket != null && ticket.IsReserved && !ticket.IsPaid && ticket.ReservationExpiresAt < DateTime.Now)
                {
                    ticket = null;
                }

                string seatStatus = "Available";

                if (ticket != null)
                {
                    if (ticket.IsPaid)
                    {
                        seatStatus = "Sold";
                    }
                    else if (ticket.IsReserved)
                    {
                        seatStatus = "Reserved";
                    }
                }

                seats.Add(new SeatAvailabilityDto
                {
                    SeatNumber = seatNumber,
                    IsAvailable = (seatStatus == "Available"),

                    PassengerName = (seatStatus == "Sold" || seatStatus == "Reserved") && ticket?.Passenger != null
                         ? $"{ticket.Passenger.FirstName} {ticket.Passenger.LastName}"
                         : null,

                    Status = seatStatus,
                    ReservationExpiresAt = (seatStatus == "Reserved") ? ticket?.ReservationExpiresAt : null,

                    // Ticket null yapıldığı için, süresi dolmuş biletin cinsiyeti "0" gidecek.
                    // Böylece yan koltuk kuralı da bozulmayacak.
                    Gender = ticket?.Passenger != null ? (int)ticket.Passenger.Gender : 0
                });
            }
            var occupiedCount = seats.Count(s => s.Status != "Available");
            var availableCount = totalSeats - occupiedCount;

            return new TripAvailabilityDto
            {
                TripId = tripId,
                TotalSeats = totalSeats,
                AvailableSeats = availableCount,
                OccupiedSeats = occupiedCount,
                Seats = seats
            };
        }

        public async Task<bool> IsSeatAvailableAsync(int tripId, int seatNumber)
        {
            await CleanExpiredReservationsAsync(tripId);

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TripId == tripId && t.SeatNumber == seatNumber);

            if (ticket == null) return true;

            // Rezerve ama süresi dolmuşsa -> BOŞ
            if (ticket.IsReserved && !ticket.IsPaid && ticket.ReservationExpiresAt < DateTime.Now)
            {
                return true;
            }
            return false;
        }

        // Yolcunun biletlerini getir
        public async Task<IEnumerable<TicketDto>> GetPassengerTicketsAsync(int passengerId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Passenger)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Bus)
                        .ThenInclude(b => b.Company)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Tickets)
                .Where(t => t.PassengerId == passengerId)
                .Where(t => t.IsPaid || (t.IsReserved && t.ReservationExpiresAt > DateTime.Now))
                .ToListAsync();

            return tickets.ToDto();
        }

        // Sefer biletlerini getir
        public async Task<IEnumerable<TicketDto>> GetTripTicketsAsync(int tripId)
        {
            var tickets = await _context.Tickets
                .Include(t => t.Passenger)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Bus)
                        .ThenInclude(b => b.Company)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Tickets)
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
                .Include(t => t.Passenger)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.OriginDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationCity)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.DestinationDistrict)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Bus)
                        .ThenInclude(b => b.Company)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.Tickets)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            return ticket?.ToDto();
        }


        // Cinsiyet Kuralı Kontrolü
        private async Task CheckGenderRuleAsync(int tripId, int seatNumber, int newPassengerGender, int? currentPassengerId = null)
        {
            // --- 2+1 OTOBÜS DÜZENİ MANTIĞI ---
            // [1][2]   [3]
            // [4][5]   [6]
            // Eğer koltuk numarası 3'ün katıysa (3, 6, 9, 12...) -> TEKLİ KOLTUKTUR.
            // Tekli koltukta cinsiyet kuralı olmaz. Direkt çık.
            if (seatNumber % 3 == 0) return;

            int neighborSeatNumber;

            // Eğer 3'e bölümünden kalan 1 ise (1, 4, 7...) -> CAM KENARI (SOL)
            // Yanındaki koltuk: Kendisi + 1
            if (seatNumber % 3 == 1)
            {
                neighborSeatNumber = seatNumber + 1;
            }
            // Eğer 3'e bölümünden kalan 2 ise (2, 5, 8...) -> KORİDOR (SOL)
            // Yanındaki koltuk: Kendisi - 1
            else
            {
                neighborSeatNumber = seatNumber - 1;
            }

            var neighborTicket = await _context.Tickets
                .Include(t => t.Passenger)
                .Where(t => t.TripId == tripId && t.SeatNumber == neighborSeatNumber)
                .Where(t => t.IsPaid || (t.IsReserved && t.ReservationExpiresAt > DateTime.Now))
                .FirstOrDefaultAsync();

            if (neighborTicket != null && neighborTicket.Passenger != null)
            {

                // Eğer yan koltukta oturan kişi, şu an işlem yapan kişiyle AYNIYSA -> İzin Ver!
                if (currentPassengerId.HasValue && neighborTicket.PassengerId == currentPassengerId.Value)
                {
                    return; // Kuralı atla, işlem başarılı.
                }

                // Aksi takdirde cinsiyet kontrolü yap
                int neighborGender = (int)neighborTicket.Passenger.Gender;

                if (neighborGender != 0 && newPassengerGender != 0 && neighborGender != newPassengerGender)
                {
                    string neighborGenderStr = neighborGender == 1 ? "Bay" : "Bayan";
                    throw new InvalidOperationException($"Seçtiğiniz koltuğun yanında bir {neighborGenderStr} yolcu oturmaktadır...");
                }
            }
        }
        public async Task<bool> ValidateSeatGenderAsync(int tripId, int seatNumber, int gender)
        {
            await CleanExpiredReservationsAsync(tripId);

            var existingTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.TripId == tripId && t.SeatNumber == seatNumber);

            if (existingTicket != null)
            {
                // Sadece "Dolu Olma" durumlarını kontrol et ve engelle.

                bool isSold = existingTicket.IsPaid;
                bool isActiveReservation = existingTicket.IsReserved && existingTicket.ReservationExpiresAt > DateTime.Now;

                // Eğer satılmışsa VEYA aktif bir rezervasyonsa -> HATA FIRLAT
                if (isSold || isActiveReservation)
                {
                    throw new InvalidOperationException($"Koltuk {seatNumber} zaten dolu. Lütfen başka bir koltuk seçiniz.");
                }
            }
            await CheckGenderRuleAsync(tripId, seatNumber, gender, null);
            return true;
        }
    }

}
