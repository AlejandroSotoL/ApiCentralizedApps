using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;

namespace CentralizedApps.Services.Interfaces
{
    public interface IMunicipalityServices
    {
        Task<ValidationResponseDto> AddMunicipalityAsync(CompleteMunicipalityDto dto);
        Task<List<GetMunicipalitysDto>> GetAllMunicipalityWithRelations();
        Task<GetMunicipalitysDto?> JustGetMunicipalityWithRelations(int MunicipalityId);
        Task<List<JustMunicipalitysDto>> justMunicipalitysDtos(int DepartamentId);
    }
}