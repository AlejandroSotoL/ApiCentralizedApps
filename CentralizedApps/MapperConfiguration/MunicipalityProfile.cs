using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Profile_AutoMapper
{
    public class MunicipalityProfile : Profile
    {
        public MunicipalityProfile()
        {
            CreateMap<Municipality, GetMunicipalitysDto>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<Theme, ThemeDto>();
            CreateMap<CourseSportsFacility, CourseSportsFacilityDto>()
                .ForMember(dest => dest.SportFacilities, opt => opt.MapFrom(src => src.SportFacilities))
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));
            CreateMap<SportsFacility, SportFacilityDto>();
            CreateMap<Course, CourseDto>();

            CreateMap<MunicipalityProcedure, MunicipalityProcedureDto>();
            CreateMap<Procedure, ProcedureDto>();
            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMediaDto>();
            CreateMap<SocialMediaType, SocialMediaTypeDto>();
            CreateMap<PaymentHistory, PaymentHistoryDto>()
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.StatusTypeNavigation));
            CreateMap<Municipality, JustMunicipalitysDto>();
            CreateMap<Availibity, AvailibityDto>();
        }
    }
}