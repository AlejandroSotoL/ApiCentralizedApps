using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos
{
    public class CourseWebDto
    {
         public GetMunicipalitysDto? municipality { get; set; } 
        public List<Course> courses { get; set; } = new();
        public List<Municipality> municipalities { get; set; } = new ();
    }
}