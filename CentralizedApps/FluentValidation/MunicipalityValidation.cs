using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using FluentValidation;

namespace CentralizedApps.FluentValidation
{
    public class MunicipalityValidation : AbstractValidator<CompleteMunicipalityDto>
    {
        public MunicipalityValidation()
        {
            // Nombre del municipio
            RuleFor(x => x.NameDto)
                .NotEmpty().WithMessage("El nombre del municipio es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del municipio no puede superar los 100 caracteres.");

            // Código de entidad
            RuleFor(x => x.EntityCodeDto)
                .NotNull().WithMessage("El código de la entidad es obligatorio.")
                .GreaterThan(0).WithMessage("El código de la entidad debe ser mayor que cero.");

            // Dominio
            RuleFor(x => x.DomainDto)
                .NotEmpty().WithMessage("El dominio es obligatorio.")
                .Matches(@"^(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$").WithMessage("El dominio no tiene un formato válido.");

            // Usuario Fintech
            RuleFor(x => x.UserFintechDto)
                .NotEmpty().WithMessage("El usuario Fintech es obligatorio.")
                .MinimumLength(5).WithMessage("El usuario Fintech debe tener al menos 5 caracteres.")
                .MaximumLength(50).WithMessage("El usuario Fintech no puede superar los 50 caracteres.");

            // Contraseña Fintech
            RuleFor(x => x.PasswordFintechDto)
                .NotEmpty().WithMessage("La contraseña Fintech es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña Fintech debe tener al menos 8 caracteres.")
                .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches(@"\d").WithMessage("La contraseña debe contener al menos un número.")
                .Matches(@"[\W_]").WithMessage("La contraseña debe contener al menos un carácter especial.");

            // Estado activo
            RuleFor(x => x.IsActiveDto)
                .NotNull().WithMessage("Debe indicar si el municipio está activo o no.");

            // Departamento
            RuleFor(x => x.DepartmentDto)
                .NotEmpty().WithMessage("El departamento es obligatorio.");

            // Tema
            RuleFor(x => x.ThemeDto)
                .NotEmpty().WithMessage("El tema es obligatorio.");
        }
    }

}