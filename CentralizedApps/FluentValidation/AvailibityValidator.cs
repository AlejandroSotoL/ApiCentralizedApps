using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class AvailibityValidator : AbstractValidator<AvailibityDto>
    {
        public AvailibityValidator()
        {
            RuleFor(availibityDto => availibityDto.TypeStatus)
                .NotEmpty()
                .WithMessage("El campo es obligatorio");
        }
    }
}