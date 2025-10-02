using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Services.ServicesWeb.Interface
{
    public interface IWeb
    {
        Task<MunicipalitiesAndSocialMediaTypeDto> MunicipalitiesAndSocialMediaType(int? id);
        Task<CourseWebDto> courses(int? id);
        Task<ValidationResponseDto> updateCourse(int id, CreateCourseDto updateCourseDto);
        Task<ValidationResponseDto> UpdateSportFacilietes(int id, CreateSportsFacilityDto updateSportsFacilityDto);
        Task<SportsFacilitiesWebDto> SportsFacilities(int? id);
    }
}