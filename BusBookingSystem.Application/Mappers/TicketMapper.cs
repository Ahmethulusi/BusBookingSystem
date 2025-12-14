using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class TicketMapper
    {
        public static TicketDto ToDto(this Ticket ticket)
        {
            // Eğer biletin kendisi yoksa null dön
            if (ticket == null) return null!;

            return new TicketDto
            {
                Id = ticket.Id,
                SeatNumber = ticket.SeatNumber,
                PaidAmount = ticket.PaidAmount,
                IsReserved = ticket.IsReserved,
                IsPaid = ticket.IsPaid,
                ReservationExpiresAt = ticket.ReservationExpiresAt,
                CreatedDate = ticket.CreatedDate,

                // --- 1. SEFER KUTUSU (Trip) ---
                Trip = ticket.Trip != null ? new TripDto
                {
                    Id = ticket.Trip.Id,
                    
                    // 👇 ID'LERİ EŞLEŞTİRELİM (Bunlar 0 geliyordu)
                    BusId = ticket.Trip.BusId,
                    OriginCityId = ticket.Trip.OriginCityId,
                    DestinationCityId = ticket.Trip.DestinationCityId,
                    // CompanyId eğer Trip tablosunda yoksa Bus üzerinden alabiliriz:
                    CompanyId = ticket.Trip.Bus != null ? ticket.Trip.Bus.CompanyId : 0,

                    // İsimler (Zaten çalışıyordu ama kontrol et)
                    OriginCityName = ticket.Trip.OriginCity?.Name ?? "Bilinmiyor",
                    DestinationCityName = ticket.Trip.DestinationCity?.Name ?? "Bilinmiyor",
                    
                    DepartureDate = ticket.Trip.DepartureDate.ToString("yyyy-MM-dd"),
                    DepartureTime = ticket.Trip.DepartureTime.ToString(),
                    Price = ticket.Trip.Price,
                    
                    CompanyName = ticket.Trip.Bus?.Company?.Name ?? "Firma Belirsiz"
                } : null!,

                // --- 2. YOLCU KUTUSU (Passenger) ---
                Passenger = ticket.Passenger != null ? new PassengerDto
                {
                    Id = ticket.Passenger.Id,
                    FirstName = ticket.Passenger.FirstName ?? "", // İsim yoksa boş string ver
                    LastName = ticket.Passenger.LastName ?? "",
                    TcNo = ticket.Passenger.TcNo,
                    Email = ticket.Passenger.Email,
                    PhoneNumber = ticket.Passenger.PhoneNumber,
                    Gender = ticket.Passenger.Gender,
                    DateOfBirth = ticket.Passenger.DateOfBirth
                } : null!
            };
        }

        public static IEnumerable<TicketDto> ToDto(this IEnumerable<Ticket> tickets)
        {
            // Liste boşsa hata vermesin
            if (tickets == null) return new List<TicketDto>();
            
            return tickets.Select(ticket => ticket.ToDto());
        }
    }
}