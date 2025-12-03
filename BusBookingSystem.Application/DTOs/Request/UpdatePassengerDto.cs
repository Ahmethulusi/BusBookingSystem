using BusBookingSystem.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.Application.DTOs
{
    public class UpdatePassengerDto
    {
        [Required(ErrorMessage = "Ad alanı gereklidir")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı gereklidir")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası gereklidir")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        [RegularExpression(@"^[\d\s\-\+\(\)]+$", ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cinsiyet seçimi gereklidir")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Doğum tarihi gereklidir")]
        public DateTime DateOfBirth { get; set; }
    }
}