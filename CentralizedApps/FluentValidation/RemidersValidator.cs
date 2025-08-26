using CentralizedApps.Models.Dtos;
using FluentValidation;
using System;

namespace CentralizedApps.FluentValidation
{
    public class RemidersValidator : AbstractValidator<CreateReminderDto>
    {
        public RemidersValidator()
        {
            RuleFor(x => x.IdProcedureMunicipality)
                .NotNull().WithMessage("El campo IdProcedureMunicipality es obligatorio.")
                .GreaterThan(0).WithMessage("El IdProcedureMunicipality debe ser mayor a 0.");

            RuleFor(x => x.ExpirationDate)
                .NotNull().WithMessage("La fecha de expiración es obligatoria.")
                .Must(date => date >= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("La fecha de expiración debe ser mayor o igual a la fecha actual.");

            RuleFor(x => x.VigenciaDate)
                .NotEmpty().WithMessage("El campo VigenciaDate es obligatorio.")
                .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("El formato de VigenciaDate debe ser YYYY-MM-DD.");

            RuleFor(x => x.ReminderType)
                .NotEmpty().WithMessage("El tipo de recordatorio es obligatorio.")
                .Must(type => new[] { "EMAIL", "SMS", "PUSH" }.Contains(type?.ToUpper()))
                .WithMessage("El tipo de recordatorio debe ser EMAIL, SMS o PUSH.");
        }
    }
}
