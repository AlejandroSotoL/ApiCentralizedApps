using System.Collections.Generic;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos
{
    public class ProceduresWebDto
    {
        public GetMunicipalitysDto? municipality { get; set; }
        public List<Course> courses { get; set; } = new();
        public List<SportsFacility> sportsFacilities { get; set; } = new();
        public List<Municipality> municipalities { get; set; } = new();
    }
}
