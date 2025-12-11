using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.DTOs.Request;
using FluentValidation;

namespace BusBookingSystem.Application.Validators
{
    public class CreatePassengerDtoValidator : AbstractValidator<CreatePassengerDto>
    {
        public CreatePassengerDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad boş olamaz")
                .MinimumLength(2).WithMessage("Ad en az 2 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Ad sadece harf içerebilir");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad boş olamaz")
                .MinimumLength(2).WithMessage("Soyad en az 2 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir")
                .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Soyad sadece harf içerebilir");

            RuleFor(x => x.TcNo)
                .NotEmpty().WithMessage("TC Kimlik No boş olamaz")
                .Matches(@"^\d{11}$").WithMessage("TC Kimlik No 11 haneli rakam olmalıdır");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası boş olamaz")
                .Matches(@"^(05)(\d{9})$").WithMessage("Telefon numarası 05XX XXX XX XX formatında olmalıdır.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Geçersiz cinsiyet değeri");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Doğum tarihi boş olamaz")
                .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Yolcu en az 18 yaşında olmalıdır")
                .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Geçersiz doğum tarihi");
        }
    }
}
