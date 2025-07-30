using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class QueryFielValidator : AbstractValidator<QueryFieldDto>
    {
        public QueryFielValidator()
        {
            RuleFor(queryFieldDto => queryFieldDto.FieldName)
            .NotEmpty()
            .WithMessage("el campo es obligatorio");
            RuleFor(queryFieldDto => queryFieldDto.MunicipalityId)
            .NotEmpty()
            .WithMessage("el campo es obligatorio")
            .Must(mayor => mayor > 0)
            .WithMessage("el id debe ser mayor a cero");
        }
    }
}