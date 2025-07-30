
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Models.Entities;
using CentralizedApps.Services.Interfaces;
using AutoMapper;
using CentralizedApps.Models.Dtos.PrincipalsDtos;

namespace CentralizedApps.Services
{
    public class ProcedureServices : IProcedureServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MunicipalityServices> _logger;
        public ProcedureServices(ILogger<MunicipalityServices> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Procedure> createProcedures(CreateProcedureDto procedureDto)
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


        public async Task<Availibity> createAvailibity(CreateAvailibityDto availibityDto)
        {
            Availibity availibity = new Availibity
            {
                TypeStatus = availibityDto.TypeStatus
            };
            await _unitOfWork.genericRepository<Availibity>().AddAsync(availibity);
            await _unitOfWork.SaveChangesAsync();
            return availibity;
        }


        public async Task<bool> AddSocialMediaType(SocialMediaTypeDto socialMediaType)
        {
            try
            {
                if (socialMediaType == null || string.IsNullOrWhiteSpace(socialMediaType.Name))
                {
                    return false;
                }

                var format = new SocialMediaType
                {
                    Name = socialMediaType.Name,
                };
                var repository = _unitOfWork.genericRepository<SocialMediaType>();
                await repository.AddAsync(format);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar SocialMediaType: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddMuncipalitySocialMediaToMunicipality(MunicipalitySocialMeditaDto_Response dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("DTO recibido es null.");
                    return false;
                }

                if (dto.MunicipalityId <= 0 || dto.SocialMediaTypeId <= 0)
                {
                    _logger.LogWarning("MunicipalityId o SocialMediaTypeId no válidos.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(dto.Url))
                {
                    _logger.LogWarning("URL de red social está vacía.");
                    return false;
                }

                var entity = _mapper.Map<MunicipalitySocialMedium>(dto);
                var repository = _unitOfWork.genericRepository<MunicipalitySocialMedium>();
                await repository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Se agregó correctamente la red social al municipio. ID Municipio: {MunicipalityId}, Tipo: {SocialMediaTypeId}", dto.MunicipalityId, dto.SocialMediaTypeId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar red social al municipio.");
                return false;
            }
        }

        public async Task<Course> createCourse(CreateCourseDto createCourseDto)
        {
            Course course = new Course
            {
                Name = createCourseDto.Name,
                Post = createCourseDto.Post,
                Get = createCourseDto.Get,
            };
            await _unitOfWork.genericRepository<Course>().AddAsync(course);
            await _unitOfWork.SaveChangesAsync();
            return course;
        }
        public async Task<SportsFacility> createSportsFacility(CreateSportsFacilityDto createSportsFacilityDto)
        {
            SportsFacility sportsFacility = new SportsFacility
            {
                Name = createSportsFacilityDto.Name,
                Get = createSportsFacilityDto.Get,
                CalendaryPost = createSportsFacilityDto.CalendaryPost,
                ReservationPost = createSportsFacilityDto.ReservationPost,
            };
            await _unitOfWork.genericRepository<SportsFacility>().AddAsync(sportsFacility);
            await _unitOfWork.SaveChangesAsync();
            return sportsFacility;
        }

        public async Task<ValidationResponseDto> updateDocumentType(int id, DocumentTypeDto updatedocumentTypeDto)
        {
            var documentType = await _unitOfWork.genericRepository<DocumentType>().GetByIdAsync(id);
            if (documentType == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            documentType.NameDocument = updatedocumentTypeDto.NameDocument;
            _unitOfWork.genericRepository<DocumentType>().Update(documentType);

            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }

        public async Task<ValidationResponseDto> updateQueryField(int id, QueryFieldDto updatequeryFieldDto)
        {
            var queryField = await _unitOfWork.genericRepository<QueryFieldDto>().GetByIdAsync(id);
            if (queryField == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            queryField.MunicipalityId = updatequeryFieldDto.MunicipalityId;
            queryField.FieldName = updatequeryFieldDto.FieldName;
            _unitOfWork.genericRepository<QueryFieldDto>().Update(queryField);

            return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = "succesfully"
                };
        }
        public async Task<ValidationResponseDto> updateAvailibity(int id, CreateAvailibityDto updateAvailibityDto)
        {
            var availibity = await _unitOfWork.genericRepository<Availibity>().GetByIdAsync(id);
            if (availibity == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            availibity.TypeStatus = updateAvailibityDto.TypeStatus;
            _unitOfWork.genericRepository<Availibity>().Update(availibity);

            return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = "succesfully"
                };
        }
        public async Task<ValidationResponseDto> updateCourse(int id, CreateCourseDto updateCourseDto)
        {
            var Course = await _unitOfWork.genericRepository<Course>().GetByIdAsync(id);
            if (Course == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            Course.Name = updateCourseDto.Name;
            Course.Post = updateCourseDto.Post;
            Course.Get = updateCourseDto.Get;
            _unitOfWork.genericRepository<Course>().Update(Course);

            return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = "succesfully"
                };
        }
        public async Task<ValidationResponseDto> updateSportsFacility(int id, CreateSportsFacilityDto updateSportsFacilityDto)
        {
            var SportsFacility = await _unitOfWork.genericRepository<SportsFacility>().GetByIdAsync(id);
            if (SportsFacility == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "NotFound"
                };
            }

            SportsFacility.Name = updateSportsFacilityDto.Name;
            SportsFacility.Get = updateSportsFacilityDto.Get;
            SportsFacility.ReservationPost = updateSportsFacilityDto.ReservationPost;
            SportsFacility.CalendaryPost = updateSportsFacilityDto.CalendaryPost;
            _unitOfWork.genericRepository<SportsFacility>().Update(SportsFacility);

            return new ValidationResponseDto
                {
                    CodeStatus = 200,
                    BooleanStatus = true,
                    SentencesError = "succesfully"
                };
        }
    }
}


