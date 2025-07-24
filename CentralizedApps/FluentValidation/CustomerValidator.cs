using FluentValidation;
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.FluentValidation
{
    public class CustomerValidator : AbstractValidator<CreateUserDto>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El primer nombre es obligatorio")
                ;

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El primer apellido es obligatorio");

            RuleFor(x => x.SecondLastName)
                .NotEmpty().WithMessage("El segundo apellido es obligatorio");

            RuleFor(x => x.NationalId)
                .NotEmpty();

            RuleFor(x => x.DocumentTypeId)
                .NotEmpty();

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("No es una dirección de correo")
                .NotEmpty().WithMessage("El Email es Obligatorio");

            RuleFor(x => x.Password)
                .MaximumLength(8)
                .NotEmpty();

            RuleFor(x => x.PhoneNumber)
            .NotNull().WithMessage("El teléfono es obligatorio")
            .Must(p => p.ToString().Length <= 10)
            .WithMessage("El teléfono debe tener máximo 6 dígitos");

            RuleFor(x => x.Address)
                .NotEmpty();

            RuleFor(x => x.BirthDate)
            .NotEmpty()
            .NotNull()
                .WithMessage("La fecha de nacimiento es obligatoria");
        }
    }
}
