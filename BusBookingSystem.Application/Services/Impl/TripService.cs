using BusBookingSystem.Application.DTOs.Request;
using BusBookingSystem.Application.DTOs.Response;
using BusBookingSystem.Application.Mappers;
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Application.Services.Impl
{
    public class TripService : ITripService
    {
        private readonly AppDbContext _context;

        public TripService(AppDbContext context)
        {
            _context = context;
        }

        // 🟢 EKLEME İŞLEMİ (AddTrip)
        public async Task<TripDto> AddTripAsync(CreateTripDto tripDto)
        {
            // 1. Validasyonlar (Bus, City, District...)
            var busExists = await _context.Buses.AnyAsync(b => b.Id == tripDto.BusId);
            if (!busExists) throw new ArgumentException($"Bus with ID {tripDto.BusId} not found.");

            var originCityExists = await _context.Cities.AnyAsync(c => c.Id == tripDto.OriginCityId);
            if (!originCityExists) throw new ArgumentException("Origin City not found.");
            
            var destinationCityExists = await _context.Cities.AnyAsync(c => c.Id == tripDto.DestinationCityId);
            if (!destinationCityExists) throw new ArgumentException("Destination City not found.");

            if (tripDto.OriginDistrictId.HasValue)
            {
                var exists = await _context.Districts.AnyAsync(d => d.Id == tripDto.OriginDistrictId.Value && d.CityId == tripDto.OriginCityId);
                if (!exists) throw new ArgumentException("Origin District doesn't match City.");
            }
            if (tripDto.DestinationDistrictId.HasValue)
            {
                var exists = await _context.Districts.AnyAsync(d => d.Id == tripDto.DestinationDistrictId.Value && d.CityId == tripDto.DestinationCityId);
                if (!exists) throw new ArgumentException("Destination District doesn't match City.");
            }
            
            // 2. GEÇMİŞ ZAMAN KONTROLÜ
            // DTO'da zaten DateOnly ve TimeOnly olduğu için direkt birleştiriyoruz. Parse gerekmez!
            DateTime tripDateTime = tripDto.DepartureDate.ToDateTime(tripDto.DepartureTime);
            
            if (tripDateTime < DateTime.Now)
            {
                throw new ArgumentException("Geçmiş bir zamana sefer ekleyemezsiniz.");
            }

            // 3. OTOBÜS MÜSAİT Mİ? (Direkt DTO verisini yolla)
            await CheckBusAvailability(tripDto.BusId, tripDto.DepartureDate, tripDto.DepartureTime);

            // 4. KAYIT (Direkt DTO verisini ata)
            var newTrip = new Trip
            {
                CompanyId = tripDto.CompanyId,
                BusId = tripDto.BusId,
                OriginCityId = tripDto.OriginCityId,
                OriginDistrictId = tripDto.OriginDistrictId,
                DestinationCityId = tripDto.DestinationCityId,
                DestinationDistrictId = tripDto.DestinationDistrictId,
                
                // 🔥 BURASI DÜZELDİ: String çevrimi yok, direkt atama var.
                DepartureDate = tripDto.DepartureDate, 
                DepartureTime = tripDto.DepartureTime, 
                
                Price = tripDto.Price
            };

            await _context.Trips.AddAsync(newTrip);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception($"VERITABANI HATASI: {realError}");
            }

            return new TripDto 
            { 
                Id = newTrip.Id,
                Price = newTrip.Price,
                DepartureDate = newTrip.DepartureDate.ToString("yyyy-MM-dd"), 
                DepartureTime = newTrip.DepartureTime.ToString("HH:mm") // 24 Saat Formatı
            };
        }

        // 🟢 LİSTELEME (GetAll)
        public async Task<IEnumerable<TripDto>> GetAllTripsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var trips = await _context.Trips
                .Include(t => t.OriginCity)
                .Include(t => t.OriginDistrict)
                .Include(t => t.DestinationCity)
                .Include(t => t.DestinationDistrict)
                .Include(t => t.Tickets)
                .Where(t => t.DepartureDate >= today)
                .OrderBy(t => t.DepartureDate)
                .ThenBy(t => t.DepartureTime)
                .ToListAsync();

            return trips.ToDto();
        }

        // 🟢 ARAMA (SearchTrips)
        public async Task<IEnumerable<TripDto>> SearchTripsAsync(int originId, int destinationId, string date)
        {
            // Gelen string tarihi DateOnly'e çeviriyoruz
            DateOnly searchDate;
            if (!DateOnly.TryParse(date, out searchDate))
            {
                return new List<TripDto>();
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            var nowTime = TimeOnly.FromDateTime(DateTime.Now);

            // Geçmiş tarih aranıyorsa boş dön
            if (searchDate < today)
            {
                return new List<TripDto>();
            }

            var query = _context.Trips
                .Include(t => t.Bus)           
                    .ThenInclude(b => b.Company)       
                .Include(t => t.OriginCity)     
                .Include(t => t.OriginDistrict)
                .Include(t => t.DestinationCity)
                .Include(t => t.DestinationDistrict)
                .Where(t => 
                    t.OriginCityId == originId && 
                    t.DestinationCityId == destinationId && 
                    t.DepartureDate == searchDate);

            // Eğer BUGÜN aranıyorsa, saati geçenleri gizle
            if (searchDate == today)
            {
                query = query.Where(t => t.DepartureTime > nowTime);
            }

            var trips = await query
                .OrderBy(t => t.DepartureTime)
                .ToListAsync();

            return trips.Select(trip => new TripDto
            {
                Id = trip.Id,
                CompanyId = trip.CompanyId, 
                BusId = trip.BusId,
                // 🔥 FİRMA İSMİ BURADA
                CompanyName = trip.Bus?.Company?.Name ?? "Firma Belirsiz", 
                
                OriginCityId = trip.OriginCityId,
                OriginCityName = trip.OriginCity?.Name ?? string.Empty,
                OriginDistrictId = trip.OriginDistrictId,
                OriginDistrictName = trip.OriginDistrict?.Name,

                DestinationCityId = trip.DestinationCityId,
                DestinationCityName = trip.DestinationCity?.Name ?? string.Empty,
                DestinationDistrictId = trip.DestinationDistrictId,
                DestinationDistrictName = trip.DestinationDistrict?.Name,

                DepartureDate = trip.DepartureDate.ToString("yyyy-MM-dd"),
                
                // 🔥 "16:00" SORUNUNUN ÇÖZÜMÜ BURADA:
                // "HH:mm" diyerek 24 saat formatını zorluyoruz. 
                // Yoksa sunucu "4:00 PM" gönderebilir.
                DepartureTime = trip.DepartureTime.ToString("HH:mm"), 
                
                Price = trip.Price
            });
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.Tickets)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null) return false;

            if (trip.Tickets.Any())
            {
                _context.Tickets.RemoveRange(trip.Tickets);
            }
            _context.Trips.Remove(trip);
            
            await _context.SaveChangesAsync();
            return true;
        }

        // 🟢 MÜSAİTLİK KONTROLÜ
        private async Task CheckBusAvailability(int busId, DateOnly date, TimeOnly newTime)
        {
            var existingTripTimes = await _context.Trips
                .Where(t => t.BusId == busId && t.DepartureDate == date)
                .Select(t => t.DepartureTime)
                .ToListAsync();

            foreach (var existingTime in existingTripTimes)
            {
                TimeSpan difference = newTime - existingTime;
                double diffHours = Math.Abs(difference.TotalHours);

                if (diffHours < 4)
                {
                    throw new InvalidOperationException(
                        $"Bu otobüsün saat {existingTime:HH:mm} civarında zaten bir seferi var. " +
                        $"Aynı otobüs 4 saat arayla sefere çıkabilir. ({newTime:HH:mm} uygun değil)");
                }
            }
        }
    }
}