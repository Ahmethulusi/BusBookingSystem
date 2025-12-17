using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class TicketMapper
    {
        public static TicketDto ToDto(this Ticket ticket)
        {
            if (ticket == null) return null!;

            // Veritabanında 'Rezerve' yazsa bile, saati geçtiyse 'Rezerve Değildir' olarak göster
            bool isActuallyReserved = ticket.IsReserved;

            if (ticket.IsReserved && ticket.ReservationExpiresAt.HasValue)
            {
                if (DateTime.Now > ticket.ReservationExpiresAt.Value)
                {
                    isActuallyReserved = false; // Kullanıcıya "Boş" göster
                }
            }

            return new TicketDto
            {
                Id = ticket.Id,
                SeatNumber = ticket.SeatNumber,
                PaidAmount = ticket.PaidAmount,
                IsReserved = isActuallyReserved,
                IsPaid = ticket.IsPaid,
                ReservationExpiresAt = ticket.ReservationExpiresAt,
                CreatedDate = ticket.CreatedDate,


                Trip = ticket.Trip?.ToDto(),

                Passenger = ticket.Passenger?.ToDto()
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