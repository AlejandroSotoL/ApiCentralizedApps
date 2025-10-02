using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos
{
    public class SportsFacilitiesWebDto
    {
        public GetMunicipalitysDto? municipality { get; set; } 
        public List<SportsFacility> sportsFacilities { get; set; } = new();
        public List<Municipality> municipalities { get; set; } = new ();
    }
}