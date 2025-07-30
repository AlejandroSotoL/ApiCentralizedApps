using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseValidator()
        {
            RuleFor(createCourse => createCourse.Name)
                .NotEmpty()
                .WithMessage("El campo es obligatorio");
            RuleFor(createCourse => createCourse.Get)
                .NotEmpty()
                .WithMessage("El campo es obligatorio")
                .Must(url => url.StartsWith("https://"))
                .WithMessage("debe iniciar con https://");
            RuleFor(createCourse => createCourse.Post)
                .NotEmpty()
                .WithMessage("El campo es obligatorio")
                .Must(url => url.StartsWith("https://"))
                .WithMessage("debe iniciar con https://");
        }
    }
}