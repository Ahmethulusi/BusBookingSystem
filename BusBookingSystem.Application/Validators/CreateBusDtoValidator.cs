using BusBookingSystem.Application.DTOs;
using BusBookingSystem.Application.DTOs.Request;
using FluentValidation;

namespace BusBookingSystem.Application.Validators
{
    public class CreateBusDtoValidator : AbstractValidator<CreateBusDto>
    {
        public CreateBusDtoValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Firma seçilmelidir");

            RuleFor(x => x.PlateNumber)
                .NotEmpty().WithMessage("Plaka boş olamaz")
                .Matches(@"^(0[1-9]|[1-7][0-9]|8[01])\s?[A-Z]{1,3}\s?\d{2,4}$")
                .WithMessage("Geçerli bir Türkiye plakası giriniz (örn: 10 FB 1907 veya 34 ABC 1234)")
                .MaximumLength(15).WithMessage("Plaka en fazla 15 karakter olabilir");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Marka boş olamaz")
                .MinimumLength(2).WithMessage("Marka en az 2 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Marka en fazla 50 karakter olabilir");

            RuleFor(x => x.TotalSeatCount)
                .GreaterThan(0).WithMessage("Koltuk sayısı 0'dan büyük olmalıdır")
                .LessThanOrEqualTo(60).WithMessage("Koltuk sayısı en fazla 60 olabilir")
                .Must(EvenNumber).WithMessage("Koltuk sayısı çift sayı olmalıdır (yan yana koltuklar için)");
        }

        private bool EvenNumber(int seatCount)
        {
            return seatCount % 2 == 0;
        }
    }
}
