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
        Task<Procedure> createProcedures(CreateProcedureDto procedureDto);
        Task<DocumentType> createDocumentType(DocumentTypeDto documentTypeDto);
        Task<QueryField> createQueryField(QueryFieldDto queryFieldDto);
        Task<Availibity> createAvailibity(CreateAvailibityDto availibityDto);
        Task<bool> createNewTheme(ThemeDto createThemeDto);
        Task<bool> AddMuncipalitySocialMediaToMunicipality(MunicipalitySocialMeditaDto_Response municipalitySocialMeditaDto_Response);

        //PUT
        Task<ValidationResponseDto> UpdateTheme(int Id, ThemeDto procedureDto);
    }
}

