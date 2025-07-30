using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos.PrincipalsDtos
{
    public class CompleteMunicipalityDto
    {
        //Municipality Dtos
        public string? NameDto { get; set; }
        public int? EntityCodeDto { get; set; }
        public string? DomainDto { get; set; }
        public string? UserFintechDto { get; set; }
        public string? PasswordFintechDto { get; set; }
        public bool? IsActiveDto { get; set; }
        //Departament Dto
        public string? DepartmentDto { get; set; }
        //Theme Dto
        public string? ThemeDto { get; set; }
    }
}