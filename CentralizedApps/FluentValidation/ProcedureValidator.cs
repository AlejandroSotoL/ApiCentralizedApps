using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class ProcedureValidator : AbstractValidator<ProcedureDto>
    {
        public ProcedureValidator()
        {
            RuleFor(procedure => procedure.Name)
                .NotEmpty()
                .WithMessage("el campo es obligatorio")
;        }
    }
}