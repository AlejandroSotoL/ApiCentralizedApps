using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class DocumentTypeValidator : AbstractValidator<DocumentTypeDto>
    {
        public DocumentTypeValidator()
        {
            RuleFor(documentTypeDto => documentTypeDto.NameDocument)
            .NotEmpty()
            .WithMessage("el campo es obligatorio");
        }
    }
}