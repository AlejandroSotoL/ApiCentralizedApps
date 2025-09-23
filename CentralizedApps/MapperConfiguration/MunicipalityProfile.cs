using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;

namespace CentralizedApps.Profile_AutoMapper
{
    public class MunicipalityProfile : Profile
    {
        public MunicipalityProfile()
        {

            //Avilibity
            CreateMap<Availibity, AvailibityDto>().ReverseMap();
            // Municipios
            CreateMap<Municipality, GetMunicipalitysDto>()
                .ForMember(dest => dest.Bank, opt => opt.MapFrom(src => src.IdBankNavigation))
                .ForMember(dest => dest.IdShield, opt => opt.MapFrom(src => src.IdShieldNavigation))
                .ReverseMap();
            CreateMap<Procedure, ProcedureDto>().ReverseMap();
            CreateMap<CreatePeopleInvitated, PeopleInvitated>().ReverseMap();

            // Reminders
            CreateMap<Reminder, ResponseReminderDto>().ReverseMap();
            CreateMap<Reminder, CreateReminderDto>().ReverseMap();

            // 🔹 Faltaban estos
            CreateMap<User, UserDto_Munucipality>().ReverseMap();
            CreateMap<MunicipalityProcedure, MunicipalityProcedureDto>().ReverseMap();
            CreateMap<MunicipalityProcedure, MunicipalityProcedureDto_Reminders>().ReverseMap();

            CreateMap<Municipality, JustMunicipalitysDto>().ReverseMap();

            // Bancos y Escudos 
            CreateMap<Bank, BankDto>().ReverseMap();
            CreateMap<ShieldMunicipality, ShieldMunicipalityDto>().ReverseMap();

            // Noticias
            CreateMap<NewsByMunicipality, NewsByMunicipalityDto>().ReverseMap();

            // Departamentos
            CreateMap<Department, DepartmentDto>().ReverseMap();

            // Temas
            CreateMap<Theme, ThemeDto>().ReverseMap();

            // Instalaciones deportivas
            CreateMap<SportsFacility, SportsFacilitiesDto>().ReverseMap();

            // Cursos
            CreateMap<Course, CourseDto>().ReverseMap();

            // Trámites
            CreateMap<MunicipalityProcedureAddDto, MunicipalityProcedure>();

            // Redes sociales
            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMediaDto>().ReverseMap();
            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMeditaDto_Response>().ReverseMap();
            CreateMap<SocialMediaType, SocialMediaTypeDto>().ReverseMap();

            // Disponibilidad
            CreateMap<Availibity, AvailibityDto>().ReverseMap();

            // QueryFields
            CreateMap<QueryField, QueryFieldDto_Relation>().ReverseMap();
        }
    }
}
