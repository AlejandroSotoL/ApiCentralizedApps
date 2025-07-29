using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
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
        public Task<bool> AddSocialMediaType(SocialMediaTypeDto socialMediaType)
        {
            throw new NotImplementedException();
        }
    }
}