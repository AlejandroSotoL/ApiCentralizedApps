using System.Reflection.Metadata;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using Microsoft.AspNetCore.Mvc;


namespace CentralizedApps.Services.Interfaces
{
    public interface IProcedureServices
    {
        Task<ValidationResponseDto> AsingProccessToMunicipality(MunicipalityProcedureAddDto addMunicipalityProcedures);
        Task<bool> AddMuncipalitySocialMediaToMunicipality(MunicipalitySocialMeditaDto_Response municipalitySocialMeditaDto_Response);
        Task<List<DocumentType>> GetDocumentTypes();


        Task<ValidationResponseDto> createReminders(CreateReminderDto createReminderDto);
        Task<ValidationResponseDto> createShield(ShieldMunicipalityDto createShieldDto);
        Task<Procedure> createProcedures(CreateProcedureDto procedureDto);
        Task<DocumentType> createDocumentType(DocumentTypeDto documentTypeDto);
        Task<QueryField> createQueryField(QueryFieldDto queryFieldDto);
        Task<Availibity> createAvailibity(CreateAvailibityDto availibityDto);
        Task<ValidationResponseDto> createNewTheme(ThemeDto createThemeDto);
        Task<ValidationResponseDto> createNewTypeProcedure( CreateProcedureDto createProcedureDto);
        Task<Course> createCourse(CreateCourseDto createCourseDto);
        Task<SocialMediaType> createSocialMediaType(CreateSocialMediaTypeDto createSocialMediaTypeDto);
        Task<SportsFacility> createSportsFacility(CreateSportsFacilityDto createSportsFacilityDto);
        Task<ValidationResponseDto> createNewNotice(NewsByMunicipalityDto newsByMunicipalityDto);


        Task<ValidationResponseDto> UpdateProcedureStatus(int Id , bool status);
        Task<ValidationResponseDto> updateNews(int id, NewsByMunicipalityDto newsByMunicipalityDto);
        Task<ValidationResponseDto> UpdateMunicipality(int Id , CompleteMunicipalityDto MunicipalityDTO);
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

