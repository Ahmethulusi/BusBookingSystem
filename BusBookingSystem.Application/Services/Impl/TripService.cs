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

        public async Task<TripDto> AddTripAsync(CreateTripDto tripDto)
        {
            // 1. OTOBÜS VE FİRMA KONTROLÜ
            var bus = await _context.Buses
                                    .Include(b => b.Company)
                                    .FirstOrDefaultAsync(b => b.Id == tripDto.BusId);

            if (bus == null) throw new ArgumentException($"ID'si {tripDto.BusId} olan otobüs bulunamadı.");

            if (bus.CompanyId != tripDto.CompanyId)
            {
                throw new ArgumentException($"Seçilen otobüs ({bus.PlateNumber}), seçilen firmaya ait değil! Lütfen doğru firma-otobüs eşleşmesi yapınız.");
            }

            // 2. City kontrolleri
            var originCityExists = await _context.Cities.AnyAsync(c => c.Id == tripDto.OriginCityId);
            if (!originCityExists) throw new ArgumentException("Kalkış şehri bulunamadı.");
            
            var destinationCityExists = await _context.Cities.AnyAsync(c => c.Id == tripDto.DestinationCityId);
            if (!destinationCityExists) throw new ArgumentException("Varış şehri bulunamadı.");

            // 3. District kontrolleri
            if (tripDto.OriginDistrictId.HasValue)
            {
                var exists = await _context.Districts.AnyAsync(d => d.Id == tripDto.OriginDistrictId.Value && d.CityId == tripDto.OriginCityId);
                if (!exists) throw new ArgumentException("Kalkış ilçesi, şehre ait değil.");
            }
            if (tripDto.DestinationDistrictId.HasValue)
            {
                var exists = await _context.Districts.AnyAsync(d => d.Id == tripDto.DestinationDistrictId.Value && d.CityId == tripDto.DestinationCityId);
                if (!exists) throw new ArgumentException("Varış ilçesi, şehre ait değil.");
            }
            
            // 4. GEÇMİŞ ZAMAN KONTROLÜ
            DateTime tripDateTime = tripDto.DepartureDate.ToDateTime(tripDto.DepartureTime);
            if (tripDateTime < DateTime.Now)
            {
                throw new ArgumentException("Geçmiş bir zamana sefer ekleyemezsiniz.");
            }

            // 5. OTOBÜS MÜSAİT Mİ?
            await CheckBusAvailability(tripDto.BusId, tripDto.DepartureDate, tripDto.DepartureTime);

            // 6. KAYIT
            var newTrip = new Trip
            {
                CompanyId = tripDto.CompanyId,
                BusId = tripDto.BusId,
                OriginCityId = tripDto.OriginCityId,
                OriginDistrictId = tripDto.OriginDistrictId,
                DestinationCityId = tripDto.DestinationCityId,
                DestinationDistrictId = tripDto.DestinationDistrictId,
                DepartureDate = tripDto.DepartureDate, 
                DepartureTime = tripDto.DepartureTime, 
                Price = tripDto.Price,
                CreatedDate = DateTime.Now
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

            // 7. KAYDEDİLEN VERİYİ GERİ ÇEK (Full Include)
            var createdTrip = await _context.Trips
                .Include(t => t.OriginCity)
                .Include(t => t.OriginDistrict)
                .Include(t => t.DestinationCity)
                .Include(t => t.DestinationDistrict)
                .Include(t => t.Bus)
                    .ThenInclude(b => b.Company)
                .FirstOrDefaultAsync(t => t.Id == newTrip.Id);

            if (createdTrip == null)
            {
                throw new Exception("Sefer oluşturuldu ancak detayları veritabanından çekilemedi.");
            }
            return new TripDto 
            { 
                Id = createdTrip.Id,
                CompanyId = createdTrip.CompanyId,
                CompanyName = createdTrip.Bus?.Company?.Name ?? "Firma Belirsiz",
                
                BusId = createdTrip.BusId,
                Price = createdTrip.Price,
                BusPlateNumber = createdTrip.Bus?.PlateNumber ?? "Plaka Yok",
                
                OriginCityId = createdTrip.OriginCityId,
                OriginCityName = createdTrip.OriginCity?.Name ?? "",
                OriginDistrictId = createdTrip.OriginDistrictId,
                OriginDistrictName = createdTrip.OriginDistrict?.Name,
                
                DestinationCityId = createdTrip.DestinationCityId,
                DestinationCityName = createdTrip.DestinationCity?.Name ?? "",
                DestinationDistrictId = createdTrip.DestinationDistrictId,
                DestinationDistrictName = createdTrip.DestinationDistrict?.Name,

                DepartureDate = createdTrip.DepartureDate.ToString("yyyy-MM-dd"), 
                DepartureTime = createdTrip.DepartureTime.ToString("HH:mm"),
                CreatedDate = createdTrip.CreatedDate
            };
        }
        //  LİSTELEME (GetAll)
        public async Task<IEnumerable<TripDto>> GetAllTripsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var trips = await _context.Trips
                .Include(t => t.Bus)           
                    .ThenInclude(b => b.Company) 
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

        //  ARAMA (SearchTrips)
        public async Task<IEnumerable<TripDto>> SearchTripsAsync(int originId, int? originDistrictId, int destinationId, int? destinationDistrictId, string date)
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

            // Temel Sorgu (Include'lar ve Zorunlu Alanlar)
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

            // 🔥 YENİ: Eğer Kalkış İlçesi seçildiyse onu da filtrele
            if (originDistrictId.HasValue)
            {
                query = query.Where(t => t.OriginDistrictId == originDistrictId.Value);
            }

            // 🔥 YENİ: Eğer Varış İlçesi seçildiyse onu da filtrele
            if (destinationDistrictId.HasValue)
            {
                query = query.Where(t => t.DestinationDistrictId == destinationDistrictId.Value);
            }

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
                CompanyName = trip.Bus?.Company?.Name ?? "Firma Belirsiz", 
                BusPlateNumber = trip.Bus?.PlateNumber ?? "Plaka Yok",
                
                OriginCityId = trip.OriginCityId,
                OriginCityName = trip.OriginCity?.Name ?? string.Empty,
                OriginDistrictId = trip.OriginDistrictId,
                OriginDistrictName = trip.OriginDistrict?.Name,

                DestinationCityId = trip.DestinationCityId,
                DestinationCityName = trip.DestinationCity?.Name ?? string.Empty,
                DestinationDistrictId = trip.DestinationDistrictId,
                DestinationDistrictName = trip.DestinationDistrict?.Name,

                DepartureDate = trip.DepartureDate.ToString("yyyy-MM-dd"),
                DepartureTime = trip.DepartureTime.ToString("HH:mm"), 
                
                Price = trip.Price,
                SoldTicketCount = trip.Tickets != null ? trip.Tickets.Count(t => t.IsPaid || t.IsReserved) : 0
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