using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.Mappers;
using BusBookingSystem.Core.Entities;
using BusBookingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Application.Services.Impl
{
    public class PassengerService : IPassengerService
    {
        private readonly AppDbContext _context;

        public PassengerService(AppDbContext context)
        {
            _context = context;
        }

        // Yolcu Ekle
        public async Task<PassengerDto> AddPassengerAsync(CreatePassengerDto passengerDto)
        {
            // Yolcu zaten mevcut mu kontrol et
            var existingPassenger = await _context.Passengers
                .FirstOrDefaultAsync(p => p.TcNo == passengerDto.TcNo || p.Email == passengerDto.Email);

            if (existingPassenger != null)
            {
                if (existingPassenger.TcNo == passengerDto.TcNo)
                    throw new InvalidOperationException($"Bu TC kimlik numarası ({passengerDto.TcNo}) ile kayıtlı yolcu zaten mevcut.");
                if (existingPassenger.Email == passengerDto.Email)
                    throw new InvalidOperationException($"Bu e-posta adresi ({passengerDto.Email}) ile kayıtlı yolcu zaten mevcut.");
            }
            // Yeni yolcu oluştur
            var newPassenger = new Passenger
            {
                FirstName = passengerDto.FirstName,
                LastName = passengerDto.LastName,
                TcNo = passengerDto.TcNo,
                Email = passengerDto.Email,
                PhoneNumber = passengerDto.PhoneNumber,
                Gender = passengerDto.Gender,
                DateOfBirth = passengerDto.DateOfBirth
            };
            await _context.Passengers.AddAsync(newPassenger);
            await _context.SaveChangesAsync();

            return newPassenger.ToDto();
        }

        // Tüm Yolcuları Çek
        public async Task<IEnumerable<PassengerDto>> GetAllPassengersAsync()
        {
            var passengers = await _context.Passengers
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync();

            return passengers.ToDto();
        }

        public async Task<PassengerDto?> GetPassengerByIdAsync(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            return passenger?.ToDto();
        }

        public async Task<PassengerDto?> GetPassengerByTcNoAsync(string tcNo)
        {
            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(p => p.TcNo == tcNo);
            return passenger?.ToDto();
        }



        public async Task<PassengerDto?> UpdatePassengerAsync(int id, UpdatePassengerDto updateDto)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null) return null;

            // Email değiştiyse, benzersizliği kontrol et
            if (passenger.Email != updateDto.Email)
            {
                var emailExists = await _context.Passengers
                    .AnyAsync(p => p.Id != id && p.Email == updateDto.Email);
                if (emailExists)
                    throw new InvalidOperationException($"Bu e-posta adresi ({updateDto.Email}) zaten kullanılmaktadır.");
            }

            // Yolcuyu Güncelle
            passenger.FirstName = updateDto.FirstName;
            passenger.LastName = updateDto.LastName;
            passenger.Email = updateDto.Email;
            passenger.PhoneNumber = updateDto.PhoneNumber;
            passenger.Gender = updateDto.Gender;
            passenger.DateOfBirth = updateDto.DateOfBirth;
            passenger.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return passenger.ToDto();
        }

        public async Task<bool> DeletePassengerAsync(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null) return false;

            // Yolcunun biletleri var mı kontrol et
            var hasTickets = await _context.Tickets.AnyAsync(t => t.PassengerId == id);
            if (hasTickets)
                throw new InvalidOperationException("Bu yolcuya ait bilet kayıtları bulunduğu için silme işlemi gerçekleştirilemez.");

            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PassengerExistsByTcNoAsync(string tcNo)
        {
            return await _context.Passengers.AnyAsync(p => p.TcNo == tcNo);
        }

        public async Task<bool> PassengerExistsByEmailAsync(string email)
        {
            return await _context.Passengers.AnyAsync(p => p.Email == email);
        }
    }
}