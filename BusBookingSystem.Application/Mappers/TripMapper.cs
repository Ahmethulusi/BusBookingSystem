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
                BusId = trip.BusId,
                OriginCityId = trip.OriginCityId,
                OriginCityName = trip.OriginCity?.Name,
                OriginDistrictId = trip.OriginDistrictId,
                OriginDistrictName = trip.OriginDistrict?.Name,
                DestinationCityId = trip.DestinationCityId,
                DestinationCityName = trip.DestinationCity?.Name,
                DestinationDistrictId = trip.DestinationDistrictId,
                DestinationDistrictName = trip.DestinationDistrict?.Name,
                DepartureDate = trip.DepartureDate,
                DepartureTime = trip.DepartureTime,
                Price = trip.Price,
                CreatedDate = trip.CreatedDate
            };
        }

        public static IEnumerable<TripDto> ToDto(this IEnumerable<Trip> trips)
        {
            return trips.Select(trip => trip.ToDto());
        }
    }
}
