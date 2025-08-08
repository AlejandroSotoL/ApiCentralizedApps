using System.Reflection.Metadata;
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
        Task<ValidationResponseDto> createNewTheme(ThemeDto createThemeDto);

        Task<ValidationResponseDto> AsingProccessToMunicipality(MunicipalityProcedureAddDto addMunicipalityProcedures);
        Task<bool> AddMuncipalitySocialMediaToMunicipality(MunicipalitySocialMeditaDto_Response municipalitySocialMeditaDto_Response);
        //GET
        Task<List<DocumentType>> GetDocumentTypes();
        //PUT

        Task<ValidationResponseDto> UpdateMunicipality(int Id , CompleteMunicipalityDto MunicipalityDTO);


        Task<ValidationResponseDto> createNewTypeProcedure( CreateProcedureDto createProcedureDto);
        Task<Course> createCourse(CreateCourseDto createCourseDto);
        Task<SocialMediaType> createSocialMediaType(CreateSocialMediaTypeDto createSocialMediaTypeDto);
        Task<SportsFacility> createSportsFacility(CreateSportsFacilityDto createSportsFacilityDto);
        Task<ValidationResponseDto> UpdateTheme(int Id, ThemeDto procedureDto);
        Task<ValidationResponseDto> updateDocumentType(int id, DocumentTypeDto documentTypeDto);
        Task<ValidationResponseDto> updateQueryField(int id, QueryFieldDto queryFieldDto);
        Task<ValidationResponseDto> updateAvailibity(int id, CreateAvailibityDto createAvailibityDto);
        Task<ValidationResponseDto> updateCourse(int id, CreateCourseDto createCourseDto);
        Task<ValidationResponseDto> updateSportsFacility(int id, CreateSportsFacilityDto updateSportsFacilityDto);
        Task<ValidationResponseDto> updateSocialMediaType(int id, CreateSocialMediaTypeDto updateSocialMediaTypeDto);
        Task<ValidationResponseDto> updateMunicipalitySocialMedium(int id, CreateMunicipalitySocialMediumDto updateMunicipalitySocialMediumDto);
    }
}

