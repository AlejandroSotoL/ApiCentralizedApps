using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class MunicipalitySocialMediumValidator :AbstractValidator<CreateMunicipalitySocialMediumDto>
    {
        public MunicipalitySocialMediumValidator()
        {
            RuleFor(unicipalitySocialMedium => unicipalitySocialMedium.MunicipalityId)
                .NotEmpty()
                .WithMessage("El campo es obligatorio")
                .GreaterThan(0)
                .WithMessage("MunicipalityId debe ser mayor a cero");
            RuleFor(unicipalitySocialMedium => unicipalitySocialMedium.SocialMediaTypeId)
                .NotEmpty()
                .WithMessage("El campo es obligatorio")
                .GreaterThan(0).
                WithMessage("SocialMediaTypeId debe ser mayor a cero");
            RuleFor(unicipalitySocialMedium => unicipalitySocialMedium.Url)
                .NotEmpty()
                .WithMessage("El campo es obligatorio")
                .Must(url => url.StartsWith("http")).
                WithMessage("Url debe empezar con http");
            RuleFor(unicipalitySocialMedium => unicipalitySocialMedium.IsActive)
                .NotEmpty()
                .WithMessage("El campo es obligatorio");    
        }
    }
}