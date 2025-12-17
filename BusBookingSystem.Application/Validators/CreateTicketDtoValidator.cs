using BusBookingSystem.Application.DTOs;
using FluentValidation;

namespace BusBookingSystem.Application.Validators
{
    public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
    {
        public CreateTicketDtoValidator()
        {
            RuleFor(x => x.PassengerId)
                .GreaterThan(0).WithMessage("Yolcu seçilmelidir");

            RuleFor(x => x.SeatNumber)
                .GreaterThan(0).WithMessage("Koltuk numarası 0'dan büyük olmalıdır");

            RuleFor(x => x.PaidAmount)
                .GreaterThan(0).WithMessage("Ödeme tutarı 0'dan büyük olmalıdır");
        }
    }

    public class ReserveTicketDtoValidator : AbstractValidator<ReserveTicketDto>
    {
        public ReserveTicketDtoValidator()
        {
            RuleFor(x => x.PassengerId)
                .GreaterThan(0).WithMessage("Yolcu seçilmelidir");

            RuleFor(x => x.SeatNumber)
                .GreaterThan(0).WithMessage("Koltuk numarası 0'dan büyük olmalıdır");
        }
    }

    public class CompleteReservationDtoValidator : AbstractValidator<CompleteReservationDto>
    {
        public CompleteReservationDtoValidator()
        {
            RuleFor(x => x.PaidAmount)
                .GreaterThan(0).WithMessage("Ödeme tutarı 0'dan büyük olmalıdır");
        }
    }
}
