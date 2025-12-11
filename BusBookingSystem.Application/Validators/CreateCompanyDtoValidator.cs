using BusBookingSystem.Application.DTOs.Request;
using FluentValidation;

namespace BusBookingSystem.Application.Validators
{
    public class CreateCompanyDtoValidator : AbstractValidator<CreateCompanyDto>
    {
        public CreateCompanyDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Firma adı boş olamaz")
                .MinimumLength(2).WithMessage("Firma adı en az 2 karakter olmalıdır")
                .MaximumLength(100).WithMessage("Firma adı en fazla 100 karakter olabilir");

            RuleFor(x => x.Phone)
                .Matches(@"^(05)(\d{9})$").WithMessage("Telefon numarası 05XX XXX XX XX formatında olmalıdır")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Adres en fazla 500 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}
