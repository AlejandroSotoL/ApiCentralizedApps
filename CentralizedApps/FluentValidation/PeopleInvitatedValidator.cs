using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using FluentValidation;
namespace CentralizedApps.FluentValidation
{
    public class PeopleInvitatedValidator : AbstractValidator<CreatePeopleInvitated>
    {
        public PeopleInvitatedValidator()
        {
            RuleFor(x => x.DocumentationDni)
                .NotEmpty().WithMessage("El documento es obligatorio");

            RuleFor(x => x.CompleteName)
                .NotEmpty().WithMessage("El nombre completo es obligatorio")
                .MaximumLength(150).WithMessage("El nombre no puede superar los 150 caracteres");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("El número de teléfono es obligatorio");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo no es válido")
                .MaximumLength(150).WithMessage("El correo no puede superar los 150 caracteres");
        }
    }
}