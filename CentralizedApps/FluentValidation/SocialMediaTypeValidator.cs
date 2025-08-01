using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class SocialMediaTypeValidator : AbstractValidator<CreateSocialMediaTypeDto>
    {
        public SocialMediaTypeValidator()
        {
            RuleFor(socialMediaType => socialMediaType.Name)
                .NotEmpty()
                .WithMessage("El campo es obligatorio");
        }
    }
}