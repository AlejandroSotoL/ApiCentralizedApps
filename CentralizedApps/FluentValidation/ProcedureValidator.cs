using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class ProcedureValidator : AbstractValidator<CreateProcedureDto>
    {
        public ProcedureValidator()
        {
            RuleFor(procedure => procedure.Name)
                .NotEmpty()
                .WithMessage("el campo es obligatorio")
;        }
    }
}