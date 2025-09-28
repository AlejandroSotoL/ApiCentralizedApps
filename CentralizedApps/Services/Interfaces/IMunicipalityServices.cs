using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IMunicipalityServices
    {
        Task<ValidationResponseDto> AddMunicipalityAsync(CompleteMunicipalityDto dto);
        Task<List<GetMunicipalitysDto>> GetAllMunicipalityWithRelations();
        Task<List<GetMunicipalitysDto>> GetAllMunicipalityWithRelationsWeb(string? filter);
        Task<GetMunicipalitysDto?> JustGetMunicipalityWithRelations(int MunicipalityId);
        Task<MunicipalityDto?> JustGetMunicipalityWithRelationsWeb(int MunicipalityId);
        Task<List<JustMunicipalitysDto>> justMunicipalitysDtos(int DepartamentId);
    }
}