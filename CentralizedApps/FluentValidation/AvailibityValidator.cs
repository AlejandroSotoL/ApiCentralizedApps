using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class AvailibityValidator : AbstractValidator<CreateAvailibityDto>
    {
        public AvailibityValidator()
        {
            RuleFor(availibityDto => availibityDto.TypeStatus)
                .NotEmpty()
                .WithMessage("El campo es obligatorio");
        }
    }
}