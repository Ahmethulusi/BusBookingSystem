using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class PassengerMapper
    {
        public static PassengerDto ToDto(this Passenger passenger)
        {
            return new PassengerDto
            {
                Id = passenger.Id,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                TcNo = passenger.TcNo,
                Email = passenger.Email,
                PhoneNumber = passenger.PhoneNumber,
                Gender = passenger.Gender,
                DateOfBirth = passenger.DateOfBirth,
                CreatedDate = passenger.CreatedDate
            };
        }

        public static IEnumerable<PassengerDto> ToDto(this IEnumerable<Passenger> passengers)
        {
            return passengers.Select(passenger => passenger.ToDto());
        }
    }
}
