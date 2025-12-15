using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.DtosFintech;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class PayerDtoValidator : AbstractValidator<PayerDto>
    {
        public PayerDtoValidator()
        {
            RuleFor(p => p.Documento)
                .NotEmpty().WithMessage("El documento es obligatorio")
                .Matches("^[0-9]+$").WithMessage("El documento solo puede contener números")
                .MaximumLength(20).WithMessage("El documento no puede exceder los 20 caracteres");

            RuleFor(p => p.TipoDocumento)
                .NotEmpty().WithMessage("El tipo de documento es obligatorio")
                .GreaterThan(0).WithMessage("El tipo de documento debe ser mayor que 0");
                
            RuleFor(p => p.Dv)
                .GreaterThanOrEqualTo(0).WithMessage("El dígito de verificación no puede ser negativo")
                .LessThan(100).WithMessage("El dígito de verificación no puede tener más de dos dígitos");

            RuleFor(p => p.PRIMERNOMBRE)
                .NotEmpty().WithMessage("El primer nombre es obligatorio")
                .MaximumLength(50).WithMessage("El primer nombre no puede exceder los 50 caracteres");

            RuleFor(p => p.PRIMERAPELLIDO)
                .NotEmpty().WithMessage("El primer apellido es obligatorio")
                .MaximumLength(50).WithMessage("El primer apellido no puede exceder los 50 caracteres");

            //RuleFor(p => p.Telefono)
            //    .NotEmpty().WithMessage("El teléfono es obligatorio")
            //    .Matches(@"^\d{7,15}$").WithMessage("El teléfono debe tener entre 7 y 15 dígitos");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("El correo no tiene un formato válido");

            //RuleFor(p => p.Direccion)
            //    .NotEmpty().WithMessage("La dirección es obligatoria")
            //    .MaximumLength(150).WithMessage("La dirección no puede exceder los 150 caracteres");
        }
    }
}
