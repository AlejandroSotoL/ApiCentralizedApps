using CentralizedApps.Models.Dtos.PrincipalsDtos;

namespace CentralizedApps.Services.Interfaces
{
    public interface IMunicipalityServices
    {
        Task<bool> AddMunicipalityAsync(CompleteMunicipalityDto dto);
        Task<List<GetMunicipalitysDto>> GetAllMunicipalityWithRelations();
        Task<GetMunicipalitysDto?> JustGetMunicipalityWithRelations(int MunicipalityId);
        Task<List<JustMunicipalitysDto>> justMunicipalitysDtos(int DepartamentId);
    }
}