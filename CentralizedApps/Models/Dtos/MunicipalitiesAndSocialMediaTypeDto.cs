using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos
{
    public class MunicipalitiesAndSocialMediaTypeDto
  {
      public GetMunicipalitysDto? municipality { get; set; } 
      public  List<SocialMediaType> socialMediaTypes { get; set; } = new();
       public List<Municipality> municipalities { get; set; } = new();
       public List<MunicipalitySocialMedium>  municipalitySocialMedia { get; set; } = new();
    }
}