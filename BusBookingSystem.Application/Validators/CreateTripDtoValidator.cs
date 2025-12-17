using BusBookingSystem.Application.DTOs.Request;
using FluentValidation;

namespace BusBookingSystem.Application.Validators
{
    public class CreateTripDtoValidator : AbstractValidator<CreateTripDto>
    {
        public CreateTripDtoValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("Firma seçilmelidir");

            RuleFor(x => x.BusId)
                .GreaterThan(0).WithMessage("Otobüs seçilmelidir");

            RuleFor(x => x.OriginCityId)
                .GreaterThan(0).WithMessage("Kalkış şehri seçilmelidir");

            RuleFor(x => x.DestinationCityId)
                .GreaterThan(0).WithMessage("Varış şehri seçilmelidir")
                .NotEqual(x => x.OriginCityId).WithMessage("Kalkış ve varış şehri aynı olamaz");

            RuleFor(x => x.DepartureDate)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Sefer tarihi bugünden önce olamaz");

            RuleFor(x => x.DepartureTime)
                .NotEmpty().WithMessage("Kalkış saati boş olamaz");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır");

            // Eğer bugünse, saat kontrolü
            RuleFor(x => x)
                .Must(x =>
                {
                    if (x.DepartureDate == DateOnly.FromDateTime(DateTime.Today))
                    {
                        var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                        return x.DepartureTime > currentTime.AddHours(1);
                    }
                    return true;
                })
                .WithMessage("Bugün için sefer en az 1 saat sonrası için oluşturulabilir");
        }
    }
}
