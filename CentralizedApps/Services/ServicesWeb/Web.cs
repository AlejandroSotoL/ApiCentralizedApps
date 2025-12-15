using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;
using CentralizedApps.Services.ServicesWeb.Interface;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Services.ServicesWeb
{
    public class Web : IWeb
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMunicipalityServices _municipalityServices;

        public Web(IUnitOfWork unitOfWork, IMunicipalityServices municipalityServices)
        {
            _unitOfWork = unitOfWork;
            _municipalityServices = municipalityServices;
        }

        public async Task<MunicipalitiesAndSocialMediaTypeDto> MunicipalitiesAndSocialMediaType(int? id)
        {
            var response = _unitOfWork.genericRepository<MunicipalitySocialMedium>();
            var Entity = await response.GetAllWithNestedIncludesAsync(query =>
                query
                    .Include(msm => msm.Municipality)
                    .Include(msm => msm.SocialMediaType)
            );

            var filtro = id.HasValue
                ? Entity.Where(m => m.MunicipalityId == id.Value).ToList()
                : new List<MunicipalitySocialMedium>();

            return new MunicipalitiesAndSocialMediaTypeDto
            {
                municipality = id.HasValue
                    ? await _municipalityServices.JustGetMunicipalityWithRelations(id.Value)
                    : null,
                municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync(),
                socialMediaTypes = await _unitOfWork.genericRepository<SocialMediaType>().GetAllAsync(),
                municipalitySocialMedia = filtro
            };

        }

        public async Task<QueryFieldWeb> QueryField(int? id)
        {
            var response = _unitOfWork.genericRepository<QueryField>();
            var Entity = await response.GetAllWithNestedIncludesAsync(query =>
                query
                    .Include(msm => msm.Municipality)

            );

            var filtro = id.HasValue
                ? Entity.Where(m => m.MunicipalityId == id.Value).ToList()
                : new List<QueryField>();

            return new QueryFieldWeb
            {
                municipality = id.HasValue
                    ? await _municipalityServices.JustGetMunicipalityWithRelations(id.Value)
                    : null,
                queryFields = filtro,
                municipalities = await _unitOfWork.genericRepository<Municipality>()
                    .GetAllWithNestedIncludesAsync(q => q.Include(m => m.Department))

            };

        }

        public async Task<ValidationResponseDto> UpdateSportFacilietes(int id, CreateSportsFacilityDto updateSportsFacilityDto)
        {
            try
            {
                var response = await _unitOfWork.genericRepository<SportsFacility>()
                    .FindAsync_Predicate(x => x.Id == id);
                if (response == null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "No se encontró el Sport."
                    };
                }
                response.IsActive = updateSportsFacilityDto.IsActive;
                response.Get = updateSportsFacilityDto.Get;
                response.CalendaryPost = updateSportsFacilityDto.CalendaryPost;
                response.ReservationPost = updateSportsFacilityDto.ReservationPost;
                response.Name = updateSportsFacilityDto.Name;
                response.MunicipalityId = updateSportsFacilityDto.MunicipalityId;
                _unitOfWork.genericRepository<SportsFacility>().Update(response);
                var rows = await _unitOfWork.SaveChangesAsync();
                if (rows > 0 && response != null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = "Estado del Spot actualizado correctamente. " + rows + " filas afectadas."
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 500,
                        SentencesError = "Error al actualizar el estado del Sport."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error al actualizar el estado del Sport: {ex.Message}"
                };
            }
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

            Course.MunicipalityId = updateCourseDto.MunicipalityId;
            Course.Name = updateCourseDto.Name;
            Course.Post = updateCourseDto.Post;
            Course.Get = updateCourseDto.Get;
            Course.IsActive = updateCourseDto.IsActive;
            _unitOfWork.genericRepository<Course>().Update(Course);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                CodeStatus = 200,
                BooleanStatus = true,
                SentencesError = "succesfully"
            };
        }

        public async Task<NewsMunicipalityDto> NewsMunicipality(int? id)
        {
            var response = _unitOfWork.genericRepository<NewsByMunicipality>();
            var Entity = await response.GetAllWithNestedIncludesAsync(query =>
                query
                    .Include(msm => msm.IdMunicipalityNavigation)

            );

            var filtro = id.HasValue
                ? Entity.Where(m => m.IdMunicipality == id.Value).ToList()
                : new List<NewsByMunicipality>();

            return new NewsMunicipalityDto
            {
                municipality = id.HasValue
                    ? await _municipalityServices.JustGetMunicipalityWithRelations(id.Value)
                    : null,
                newsByMunicipalities = filtro,
                municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync()

            };
        }

        public async Task<ProceduresWebDto> Procedures(int? id)
        {
            var coursesResponse = _unitOfWork.genericRepository<Course>();
            var coursesEntity = await coursesResponse.GetAllWithNestedIncludesAsync(query =>
                query.Include(msm => msm.Municipality));

            var sportsResponse = _unitOfWork.genericRepository<SportsFacility>();
            var sportsEntity = await sportsResponse.GetAllWithNestedIncludesAsync(query =>
                query.Include(msm => msm.Municipality));

            var coursesFiltro = id.HasValue
                ? coursesEntity.Where(m => m.MunicipalityId == id.Value).ToList()
                : new List<Course>();

            var sportsFiltro = id.HasValue
                ? sportsEntity.Where(m => m.MunicipalityId == id.Value).ToList()
                : new List<SportsFacility>();

            return new ProceduresWebDto
            {
                municipality = id.HasValue
                    ? await _municipalityServices.JustGetMunicipalityWithRelations(id.Value)
                    : null,
                courses = coursesFiltro,
                sportsFacilities = sportsFiltro,
                municipalities = await _unitOfWork.genericRepository<Municipality>().GetAllAsync()
            };
        }

    }
}