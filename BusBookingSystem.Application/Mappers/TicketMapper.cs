using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class TicketMapper
    {
        public static TicketDto ToDto(this Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                TripId = ticket.TripId,
                PassengerId = ticket.PassengerId,
                PassengerName = $"{ticket.Passenger.FirstName} {ticket.Passenger.LastName}",
                SeatNumber = ticket.SeatNumber,
                PaidAmount = ticket.PaidAmount,
                IsReserved = ticket.IsReserved,
                IsPaid = ticket.IsPaid,
                ReservationExpiresAt = ticket.ReservationExpiresAt,
                CreatedDate = ticket.CreatedDate,
                Origin = ticket.Trip.OriginCity.Name,
                Destination = ticket.Trip.DestinationCity.Name,
                DepartureDate = ticket.Trip.DepartureDate.ToString("yyyy-MM-dd"),
                TripPrice = ticket.Trip.Price
            };
        }

        public static IEnumerable<TicketDto> ToDto(this IEnumerable<Ticket> tickets)
        {
            return tickets.Select(ticket => ticket.ToDto());
        }
    }
}
