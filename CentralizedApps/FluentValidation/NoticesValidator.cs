using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class NoticesValidator :  AbstractValidator<NewsByMunicipalityDto>
    {
        public NoticesValidator()
        {
            RuleFor(url => url.GetUrlNew)
            .NotEmpty()
            .WithMessage("El campo es obligatorio")
            .Must(url => url!.StartsWith("http"))
            .WithMessage("debe iniciar con http");

            RuleFor(identity => identity.IdMunicipality)
            .NotEmpty()
            .WithMessage("El campo es obligatorio");
        }
    }
}