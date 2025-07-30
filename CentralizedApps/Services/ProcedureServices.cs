using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Services.Interfaces;
using AutoMapper;

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

        public async Task<bool> createCurseSports(AddCourseSportsFacilityDto courseSportsFacilityDto)
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

    }
}

