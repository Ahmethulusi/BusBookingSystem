using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class BusMapper
    {
        public static BusDto ToDto(this Bus bus)
        {
            return new BusDto
            {
                Id = bus.Id,
                PlateNumber = bus.PlateNumber,
                Brand = bus.Brand,
                TotalSeatCount = bus.TotalSeatCount,
                CompanyId = bus.CompanyId,
                CreatedDate = bus.CreatedDate
            };
        }

        public static IEnumerable<BusDto> ToDto(this IEnumerable<Bus> buses)
        {
            return buses.Select(bus => bus.ToDto());
        }
    }
}
