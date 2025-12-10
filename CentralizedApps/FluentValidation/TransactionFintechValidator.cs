using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.DtosFintech;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class TransactionFintechValidator : AbstractValidator<TransactionFintech>
    {
        public TransactionFintechValidator()
        {
            RuleFor(t => t.IdTramite)
                .NotEmpty().WithMessage("El campo IdTramite no puede estar vacío")
                .GreaterThan(0).WithMessage("El IdTramite debe ser mayor que 0");

            RuleFor(t => t.Pagador)
                .NotNull().WithMessage("El pagador es obligatorio")
                .SetValidator(new PayerDtoValidator());

            RuleFor(t => t.FuentePago)
                .NotEmpty().WithMessage("La fuente de pago no puede estar vacía")
                .GreaterThan(0).WithMessage("La fuente de pago debe ser mayor que 0");

            RuleFor(t => t.TipoImplementacion)
                .NotEmpty().WithMessage("El tipo de implementación no puede estar vacío")
                .GreaterThan(0).WithMessage("El tipo de implementación debe ser mayor que 0");

            // RuleFor(t => t.Estado_Url)
            //     .NotNull().WithMessage("El estado de la URL debe estar especificado");

            // RuleFor(t => t.Url)
            //     .NotEmpty().WithMessage("La URL no puede estar vacía")
            //     .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            //         .WithMessage("La URL no es válida")
            //     .Must(url => url.StartsWith("https://"))
            //         .WithMessage("La URL debe ser segura (HTTPS)")
            //     .MaximumLength(300)
            //         .WithMessage("La URL no puede exceder los 200 caracteres");

            RuleFor(t => t.ValorPagar)
                .NotEmpty().WithMessage("El valor a pagar no puede estar vacío")
                .GreaterThan(0).WithMessage("El valor a pagar debe ser mayor que 0");

            RuleFor(t => t.Factura)
                .NotEmpty().WithMessage("La factura no puede estar vacía")
                .MaximumLength(10).WithMessage("La factura no puede exceder los 50 caracteres");

            RuleFor(t => t.referencia)
                .NotEmpty().WithMessage("La referencia no puede estar vacía")
                .MaximumLength(100).WithMessage("La referencia no puede exceder los 100 caracteres");

            RuleFor(t => t.Descripcion)
                .NotEmpty().WithMessage("La descripción no puede estar vacía")
                .MaximumLength(200).WithMessage("La descripción no puede exceder los 200 caracteres");
        }
    }
}