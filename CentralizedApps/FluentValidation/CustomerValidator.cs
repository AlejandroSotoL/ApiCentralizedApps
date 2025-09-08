using FluentValidation;
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.FluentValidation
{
    public class CustomerValidator : AbstractValidator<UserDto>
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
                .NotEmpty();s

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El Email es obligatorio")
                .Must(email => email.Contains("@"))
                .WithMessage("el campo debe tener formato mail");
                

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña no puede estar vacía")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres");

            RuleFor(x => x.PhoneNumber)
            .NotNull().WithMessage("El teléfono es obligatorio")
            .Must(p => p?.ToString().Length <= 10)
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
