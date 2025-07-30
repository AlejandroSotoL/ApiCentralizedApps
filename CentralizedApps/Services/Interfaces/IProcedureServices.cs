using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IProcedureServices
    {
        Task<bool> createCurseSports(AddCourseSportsFacilityDto courseSportsFacilityDto);
        Task<bool> AddSocialMediaType(SocialMediaTypeDto socialMediaType);
        Task<bool> AddMuncipalitySocialMediaToMunicipality(MunicipalitySocialMeditaDto_Response municipalitySocialMeditaDto_Response);
    }
}