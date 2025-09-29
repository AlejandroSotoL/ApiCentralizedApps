using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos.PrincipalsDtos
{
    public class MunicipalityDto
    {
        public GetMunicipalitysDto? municipality { get; set; }
        public ShieldMunicipality? shieldMunicipality { get; set; }
        public List<Bank> Banks { get; set; } = new();
        public ShieldMunicipality? shields { get; set; }
        public List<Department> Departments { get; set; } = new();
        public List<Theme> Themes { get; set; } = new();
        public Course Courses { get; set; } = new();
        public SportsFacility SportsFacilities { get; set; } = new();
        public List<MunicipalityProcedure> MunicipalityProcedures { get; set; } = new();
        public List<MunicipalitySocialMedium> MunicipalitySocialMedia { get; set; } = new();
        public List<QueryField> QueryFields { get; set; } = new();
        public NewsByMunicipality NewsByMunicipalities { get; set; } = new();
    }
}