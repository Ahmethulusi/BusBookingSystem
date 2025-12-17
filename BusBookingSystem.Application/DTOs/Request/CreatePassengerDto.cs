using BusBookingSystem.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.Application.DTOs
{
    public class CreatePassengerDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string TcNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}