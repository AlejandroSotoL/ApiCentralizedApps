
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Models.Entities;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services
{
    public class ProcedureServices : IProcedureServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProcedureServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> createCurseSports(CourseSportsFacilityDto courseSportsFacilityDto)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {

                Course course = new Course
                {
                    Name = courseSportsFacilityDto?.courseDto?.Name,
                    Get = courseSportsFacilityDto?.courseDto?.Get,
                    Post = courseSportsFacilityDto?.courseDto?.Post
                };

                SportsFacility sportsFacility = new SportsFacility
                {
                    Name = courseSportsFacilityDto?.sportsFacilityDto?.Name,
                    Get = courseSportsFacilityDto?.sportsFacilityDto?.Get,
                    ReservationPost = courseSportsFacilityDto?.sportsFacilityDto?.ReservationPost,
                    CalendaryPost = courseSportsFacilityDto?.sportsFacilityDto?.CalendaryPost
                };



                await _unitOfWork.genericRepository<Course>().AddAsync(course);
                await _unitOfWork.genericRepository<SportsFacility>().AddAsync(sportsFacility);

                await _unitOfWork.SaveChangesAsync();

                CourseSportsFacility courseSportsFacility = new CourseSportsFacility
                {
                    SportFacilitiesId = sportsFacility.Id,
                    CoursesId = course.Id,
                    MunicipalityId = courseSportsFacilityDto?.MunicipalityId
                };

                await _unitOfWork.genericRepository<CourseSportsFacility>().AddAsync(courseSportsFacility);

                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        

        public async Task<Procedure> createProcedures(ProcedureDto procedureDto)
        {
            Procedure procedure = new Procedure
            {
                Name = procedureDto.Name
            };
            await _unitOfWork.genericRepository<Procedure>().AddAsync(procedure);
            await _unitOfWork.SaveChangesAsync();
            return procedure;
        }

        public async Task<DocumentType> createDocumentType(DocumentTypeDto documentTypeDto)
        {
            DocumentType documentType = new DocumentType
            {
                NameDocument = documentTypeDto.NameDocument
            };
            await _unitOfWork.genericRepository<DocumentType>().AddAsync(documentType);
            await _unitOfWork.SaveChangesAsync();
            return documentType;
        }
        public async Task<QueryField> createQueryField(QueryFieldDto queryFieldDto)
        {
            QueryField queryField = new QueryField
            {
                FieldName = queryFieldDto.FieldName
            };
            await _unitOfWork.genericRepository<QueryField>().AddAsync(queryField);
            await _unitOfWork.SaveChangesAsync();
            return queryField;
        }
        public async Task<Availibity> createAvailibity(AvailibityDto availibityDto)
        {
            Availibity availibity = new Availibity
            {
                TypeStatus = availibityDto.TypeStatus
            };
            await _unitOfWork.genericRepository<Availibity>().AddAsync(availibity);
            await _unitOfWork.SaveChangesAsync();
            return availibity;
        }

        
    }
}