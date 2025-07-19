using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Application.DTOS;
using FluentValidation;

namespace CentralizedApps.Infrastructure.FluentValidation
{
    public class LoginValidator : AbstractValidator<LoginUserDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El campo es requerido")
                .EmailAddress().WithMessage("No es una dirección de correo");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("El campo es requerido")
                .MaximumLength(8);
        }
    }
}