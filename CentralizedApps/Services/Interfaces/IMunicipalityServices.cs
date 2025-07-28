using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos.PrincipalsDtos;

namespace CentralizedApps.Services.Interfaces
{
    public interface IMunicipalityServices
    {
        Task<bool> AddMunicipalityAsync(CompleteMunicipalityDto dto);
        Task<List<GetMunicipalitysDto>> GetAllMunicipalityWithRelations();
        Task<List<JustMunicipalitysDto>> justMunicipalitysDtos(int DepartamentId);
    }
}