using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class SportsFacilityvalidator : AbstractValidator<CourseSportsFacilityDto>
    {
        public SportsFacilityvalidator()
        {
            RuleFor(courseSportsFacilityDto => courseSportsFacilityDto.MunicipalityId)
                .NotEmpty()
                .WithErrorCode("el id del municipio debe ser obligatorio")
                .Must(mayor => mayor > 0)
                .WithMessage("el id debe ser mayor a cero");
                

            RuleFor(courseSportsFacilityDto => courseSportsFacilityDto.courseDto.Name)
                .NotEmpty()
                .WithMessage("el nombre es obligatorio");

            RuleFor(courseSportsFacilityDto => courseSportsFacilityDto.courseDto.Get)
                .NotEmpty()
                .WithMessage("el link  es obligatorio")
                .Must(link => link.StartsWith("https://"))
            .WithMessage("El link debe iniciar con https://");

            RuleFor(courseSportsFacilityDto => courseSportsFacilityDto.courseDto.Post)
                .NotEmpty()
                .WithMessage("el link es obligatorio")
                .Must(link => link.StartsWith("https://"))
                .WithMessage("El link debe iniciar con https://");

            RuleFor(courseSportsFacilityDto => courseSportsFacilityDto.sportsFacilityDto.Get)
                .NotEmpty()
                .WithMessage("el link es obligatorio")
                .Must(link => link.StartsWith("https://"))
                .WithMessage("El link debe iniciar con https://");

            RuleFor(courseSportsFacilityDto => courseSportsFacilityDto.sportsFacilityDto.ReservationPost)
                .NotEmpty()
                .WithMessage("el link es obligatorio")
                .Must(link => link.StartsWith("https://"))
                .WithMessage("El link debe iniciar con https://");
                
            RuleFor(courseSportsFacilityDto => courseSportsFacilityDto.sportsFacilityDto.CalendaryPost)
                .NotEmpty()
                .WithMessage("el link es obligatorio")
                .Must(link => link.StartsWith("https://"))
                .WithMessage("El link debe iniciar con https://");

        }
    }
}