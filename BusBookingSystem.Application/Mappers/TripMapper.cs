using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Core.Entities;

namespace BusBookingSystem.Application.Mappers
{
    public static class TripMapper
    {
        public static TripDto ToDto(this Trip trip)
        {
            return new TripDto
            {
                Id = trip.Id,
                CompanyId = trip.CompanyId,
                BusId = trip.BusId,
                BusPlateNumber = trip.Bus?.PlateNumber ?? string.Empty,
                OriginCityId = trip.OriginCityId,
                OriginCityName = trip.OriginCity?.Name ?? string.Empty,
                OriginDistrictId = trip.OriginDistrictId,
                OriginDistrictName = trip.OriginDistrict?.Name ?? null,
                DestinationCityId = trip.DestinationCityId,
                DestinationCityName = trip.DestinationCity?.Name ?? string.Empty,
                DestinationDistrictId = trip.DestinationDistrictId,
                DestinationDistrictName = trip.DestinationDistrict?.Name,
                DepartureDate = trip.DepartureDate.ToString("yyyy-MM-dd"),
                DepartureTime = trip.DepartureTime.ToString("HH:mm:ss"),
                Price = trip.Price,
                CreatedDate = trip.CreatedDate,
                CompanyName = trip.Bus?.Company?.Name ?? string.Empty,
                SoldTicketCount = trip.Tickets != null ? trip.Tickets.Count : 0
            };
        }

        public static IEnumerable<TripDto> ToDto(this IEnumerable<Trip> trips)
        {
            return trips.Select(trip => trip.ToDto());
        }
    }
}
