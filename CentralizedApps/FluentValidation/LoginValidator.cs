
using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class LoginValidator : AbstractValidator<LoginUserDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                       .NotEmpty().WithMessage("El email es obligatorio.")
                       .EmailAddress().WithMessage("Formato de email inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}