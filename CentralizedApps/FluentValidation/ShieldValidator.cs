using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class ShieldValidator : AbstractValidator<ShieldMunicipalityDto>
    {
        public ShieldValidator()
        {
            RuleFor(x => x.Url)
    .NotEmpty().WithMessage("El dominio es obligatorio.")
    .Must(url => url.StartsWith("http")).WithMessage("El dominio no tiene un formato válido.");

            RuleFor(x => x.NameOfMuniciopality)
        .NotEmpty().WithMessage("El dominio es obligatorio.");
        }
    }
}