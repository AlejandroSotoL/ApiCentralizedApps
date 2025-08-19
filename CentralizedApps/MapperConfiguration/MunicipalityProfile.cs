using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Profile_AutoMapper
{
    public class MunicipalityProfile : Profile
    {
        public MunicipalityProfile()
        {
            CreateMap<CreateBankDto, Bank>();
            CreateMap<ShieldMunicipalityDto , ShieldMunicipality>();
            CreateMap<NewsByMunicipalityDto, NewsByMunicipality>();
            CreateMap<NewsByMunicipalityDto, NewsByMunicipality>().ReverseMap();
            CreateMap<Department, DepartmentDto>();
            CreateMap<Theme, ThemeDto>().ReverseMap();
            CreateMap<SportsFacility, SportsFacilitiesDto>();
            CreateMap<Municipality, GetMunicipalitysDto>().ReverseMap();
            CreateMap<Course, CourseDto>();
            CreateMap<MunicipalityProcedure, MunicipalityProcedureDto>();
            CreateMap<Procedure, ProcedureDto>();
            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMediaDto>();
            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMeditaDto_Response>().ReverseMap();
            CreateMap<SocialMediaType, SocialMediaTypeDto>();
            CreateMap<Availibity, AvailibityDto>();
            CreateMap<Municipality, JustMunicipalitysDto>();
            CreateMap<QueryField, QueryFieldDto_Relation>();
            CreateMap<MunicipalityProcedureAddDto, MunicipalityProcedure>();
        }
    }
}