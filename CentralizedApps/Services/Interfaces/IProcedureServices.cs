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
        Task<bool> AddMuncipalitySocialMediaToMunicipality(MunicipalitySocialMeditaDto_Response municipalitySocialMeditaDto_Response);
        Task<Course> createCourse(CreateCourseDto createCourseDto);
        Task<SportsFacility> createSportsFacility(CreateSportsFacilityDto createSportsFacilityDto);
        Task<ValidationResponseDto> updateDocumentType(int id, DocumentTypeDto documentTypeDto);
        Task<ValidationResponseDto> updateQueryField(int id, QueryFieldDto queryFieldDto);
        Task<ValidationResponseDto> updateAvailibity(int id, CreateAvailibityDto createAvailibityDto);
        Task<ValidationResponseDto> updateCourse(int id, CreateCourseDto createCourseDto);
        Task<ValidationResponseDto> updateSportsFacility(int id, CreateSportsFacilityDto updateSportsFacilityDto);
    }
}