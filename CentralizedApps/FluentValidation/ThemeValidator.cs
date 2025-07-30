using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class ThemeValidator : AbstractValidator<ThemeDto>
    {
        public ThemeValidator()
        {
            RuleFor(x => x.BackGroundColor)
                .NotEmpty().WithMessage("El color de fondo es obligatorio.")
                .Length(7).WithMessage("El color de fondo debe tener 7 caracteres (ejemplo: #FFFFFF).");

            RuleFor(x => x.Shield)
                .NotEmpty().WithMessage("El escudo es obligatorio.");

            RuleFor(x => x.PrimaryColor)
                .NotEmpty().WithMessage("El color primario es obligatorio.")
                .Length(7).WithMessage("El color primario debe tener 7 caracteres (ejemplo: #FFFFFF).");

            RuleFor(x => x.SecondaryColor)
                .NotEmpty().WithMessage("El color secundario es obligatorio.")
                .Length(7).WithMessage("El color secundario debe tener 7 caracteres (ejemplo: #FFFFFF).");

            RuleFor(x => x.SecondaryColorBlack)
                .NotEmpty().WithMessage("El color secundario (negro) es obligatorio.")
                .Length(7).WithMessage("El color secundario (negro) debe tener 7 caracteres (ejemplo: #000000).");

            RuleFor(x => x.OnPrimaryColorLight)
                .NotEmpty().WithMessage("El color primario claro es obligatorio.")
                .Length(7).WithMessage("El color primario claro debe tener 7 caracteres (ejemplo: #FFFFFF).");

            RuleFor(x => x.OnPrimaryColorDark)
                .NotEmpty().WithMessage("El color primario oscuro es obligatorio.")
                .Length(7).WithMessage("El color primario oscuro debe tener 7 caracteres (ejemplo: #000000).");
        }
    }
}
