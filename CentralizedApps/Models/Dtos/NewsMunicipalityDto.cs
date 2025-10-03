using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos
{
    public class NewsMunicipalityDto
    {
        public GetMunicipalitysDto? municipality { get; set; } 
        public List<Municipality> municipalities { get; set; } = new();
        public List<NewsByMunicipality> newsByMunicipalities { get; set; } = new();
    }
}