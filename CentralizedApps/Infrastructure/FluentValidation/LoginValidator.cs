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
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(8);
        }
    }
}