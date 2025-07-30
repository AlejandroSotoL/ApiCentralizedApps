using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class CreateSportsFacilityvalidator : AbstractValidator<CreateSportsFacilityDto>
    {
        public CreateSportsFacilityvalidator()
        {
            RuleFor(CreateSportsFacility => CreateSportsFacility.Name)
            .NotEmpty()
            .WithMessage("el campo es obligatorio");
            RuleFor(CreateSportsFacility => CreateSportsFacility.CalendaryPost)
            .NotEmpty()
            .WithMessage("el campo es obligatorio")
            .Must(url => url.StartsWith("http"))
            .WithMessage("el campo debe inicar con http");
            RuleFor(CreateSportsFacility => CreateSportsFacility.ReservationPost)
            .NotEmpty()
            .WithMessage("el campo es obligatorio")
            .Must(url => url.StartsWith("http"))
            .WithMessage("el campo debe inicar con http");
            RuleFor(CreateSportsFacility => CreateSportsFacility.Get)
            .NotEmpty()
            .WithMessage("el campo es obligatorio")
            .Must(url => url.StartsWith("http"))
            .WithMessage("el campo debe inicar con http");

        }
    }
}