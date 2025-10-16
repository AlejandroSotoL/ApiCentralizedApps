using System.Reflection.Emit;
using AutoMapper;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;
using Microsoft.Data.SqlClient;

namespace CentralizedApps.Profile_AutoMapper
{
    public class MunicipalityProfile : Profile
    {
        public MunicipalityProfile()
        {
            CreateMap<PaymentHistory, CompletePaymentDto>()
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => src.StatusTypeNavigation))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.MunicipalityProcedure, opt => opt.MapFrom(src => src.MunicipalityProcedures));
            CreateMap<User, UserDtoHistory>();
            CreateMap<MunicipalityProcedure, MunicipalityProcedureDtoPayment>();

            CreateMap<Availibity, AvailibityDto>().ReverseMap();
            CreateMap<Municipality, GetMunicipalitysDto>()
                .ForMember(dest => dest.Bank, opt => opt.MapFrom(src => src.IdBankNavigation))
                .ForMember(dest => dest.IdShield, opt => opt.MapFrom(src => src.IdShieldNavigation))
                .ReverseMap();
            CreateMap<Procedure, ProcedureDto>().ReverseMap();
            CreateMap<CreatePeopleInvitated, PeopleInvitated>().ReverseMap();

            CreateMap<Reminder, ResponseReminderDto>().ReverseMap();
            CreateMap<Reminder, CreateReminderDto>().ReverseMap();

            CreateMap<User, UserDto_Munucipality>().ReverseMap();
            CreateMap<MunicipalityProcedure, MunicipalityProcedureDto>().ReverseMap();
            CreateMap<MunicipalityProcedure, MunicipalityProcedureDto_Reminders>().ReverseMap();

            CreateMap<Municipality, JustMunicipalitysDto>().ReverseMap();

            CreateMap<Bank, BankDto>().ReverseMap();
            CreateMap<Bank, CreateBankDto>().ReverseMap();
            CreateMap<ShieldMunicipality, ShieldMunicipalityDto>().ReverseMap();

            CreateMap<NewsByMunicipality, NewsByMunicipalityDto>().ReverseMap();

            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();

            CreateMap<Theme, ThemeDto>().ReverseMap();

            CreateMap<SportsFacility, SportsFacilitiesDto>().ReverseMap();

            CreateMap<Course, CourseDto>().ReverseMap();

            CreateMap<MunicipalityProcedureAddDto, MunicipalityProcedure>();

            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMediaDto>().ReverseMap();
            CreateMap<MunicipalitySocialMedium, MunicipalitySocialMeditaDto_Response>().ReverseMap();
            CreateMap<SocialMediaType, SocialMediaTypeDto>().ReverseMap();

            CreateMap<Availibity, AvailibityDto>().ReverseMap();

            CreateMap<QueryField, QueryFieldDto_Relation>().ReverseMap();
            CreateMap<Municipality, Municipality>().ReverseMap();

            CreateMap<Municipality, MunicipalityDto>()
            .ForMember(dest => dest.municipality, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses.FirstOrDefault(c => c.MunicipalityId == src.Id) ?? new Course()))
            .ForMember(dest => dest.NewsByMunicipalities, opt => opt.MapFrom(src => src.NewsByMunicipalities.FirstOrDefault(c => c.IdMunicipality == src.Id) ?? new NewsByMunicipality()))
            .ForMember(dest => dest.SportsFacilities, opt => opt.MapFrom(src => src.SportsFacilities.FirstOrDefault(c => c.MunicipalityId == src.Id) ?? new SportsFacility()));
        }
    }
}
