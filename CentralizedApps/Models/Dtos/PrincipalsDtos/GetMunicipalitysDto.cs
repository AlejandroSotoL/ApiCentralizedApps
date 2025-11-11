
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos.PrincipalsDtos
{
    public class GetMunicipalitysDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EntityCode { get; set; }
        public bool? IsActive { get; set; }
        public string? Domain { get; set; }
        public string? UserFintech { get; set; }
        public string? PasswordFintech { get; set; }
        public string? DataPrivacy { get; set; }
        public string? DataProcessingPrivacy { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? EmailMunicipalities { get; set; }
        public string? EmailPanic { get; set; }
        public string? Phone { get; set; }
        public BankDto? Bank { get; set; }
        public ShieldMunicipalityDto? IdShield { get; set; }
        public DepartmentDto? Department { get; set; }
        public ThemeDto? Theme { get; set; }
        public List<CourseDto>? Courses { get; set; }
        public List<SportsFacilitiesDto>? SportsFacilities { get; set; }
        public List<MunicipalityProcedureDto>? MunicipalityProcedures { get; set; }
        public List<MunicipalitySocialMediaDto>? MunicipalitySocialMedia { get; set; }
        public List<QueryFieldDto_Relation>? QueryFields { get; set; }
        public List<NewsByMunicipalityDto>? NewsByMunicipalities { get; set; }
    }

    public class BankDto
    {
        public string NameBank { get; set; }
    }

    public class ShieldMunicipalityDto
    {
        public string NameOfMunicipality { get; set; }
        public string Url { get; set; }
    }

    public class QueryFieldDto_Relation
    {
        public int Id { get; set; }
        public string? FieldName { get; set; }
        public string? QueryFieldType { get; set; }

    }

    public class NewsByMunicipalityDto
    {
        public string? GetUrlNew { get; set; }
        public int? IdMunicipality { get; set; }
    }

    public class DepartmentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class UserDto_Munucipality
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ThemeDto
    {

        public string? NameTheme { get; set; }

        public string? BackGroundColor { get; set; }

        public string? PrimaryColor { get; set; }

        public string? SecondaryColor { get; set; }

        public string? SecondaryColorBlack { get; set; }

        public string? OnPrimaryColorLight { get; set; }

        public string? OnPrimaryColorDark { get; set; }
    }


    public class SportsFacilitiesDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Get { get; set; }
        public bool? IsActive { get; set; }
        public string? CalendaryPost { get; set; }
        public string? ReservationPost { get; set; }

    }

    public class CourseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Get { get; set; }
        public string? Post { get; set; }
        public bool? IsActive { get; set; }
    }

    public class MunicipalityProcedureDto
    {
        public int Id { get; set; }
        public string IntegrationType { get; set; }
        public bool? IsActive { get; set; }
        public ProcedureDto? Procedures { get; set; }
    }

    public class ProcedureDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class MunicipalitySocialMediaDto
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public bool? IsActive { get; set; }
        public SocialMediaTypeDto? SocialMediaType { get; set; }
    }

    public class SocialMediaTypeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    // public class PaymentHistoryDto
    // {
    //     public int Id { get; set; }
    //     public decimal Amount { get; set; }
    //     public DateTime Date { get; set; }
    //     public JustMunicipalitysDto? Municipality { get; set; }
    //     public MunicipalityProcedureDto? MunicipalityProcedures { get; set; }
    //     public AvailibityDto? Availability { get; set; }
    // }

    public class AvailibityDto
    {
        public int Id { get; set; }
        public string? TypeStatus { get; set; }
    }
}