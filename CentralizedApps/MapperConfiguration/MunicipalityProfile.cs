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
            CreateMap<Theme, ThemeDto>().ReverseMap();
            CreateMap<SportsFacility, SportsFacilitiesDto>();
            CreateMap<Course, CourseDto>();
            CreateMap<MunicipalityProcedure, MunicipalityProcedureDto>();
            CreateMap<Procedure, ProcedureDto>();
            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMediaDto>();
            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMeditaDto_Response>().ReverseMap();
            CreateMap<SocialMediaType, SocialMediaTypeDto>();
            CreateMap<PaymentHistory, PaymentHistoryDto>()
                .ForMember(dest => dest.Availability, opt => opt.MapFrom(src => src.StatusTypeNavigation));
            CreateMap<Municipality, JustMunicipalitysDto>();
            CreateMap<Availibity, AvailibityDto>();
        }
    }
}