using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos.PrincipalsDtos
{
    public class GetMunicipalitysDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? EntityCode { get; set; }
        public bool? IsActive { get; set; }
        public string? Domain { get; set; }
        public string? UserFintech { get; set; }
        public string? PasswordFintech { get; set; }

        public DepartmentDto? Department { get; set; }
        public ThemeDto? Theme { get; set; }

        public List<CourseSportsFacilityDto>? CourseSportsFacilities { get; set; }
        public List<MunicipalityProcedureDto>? MunicipalityProcedures { get; set; }
        public List<MunicipalitySocialMediaDto>? MunicipalitySocialMedia { get; set; }
        public List<PaymentHistoryDto>? PaymentHistories { get; set; }
    }

    public class DepartmentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class UserDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
    }

    public class ThemeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? BackGroundColor { get; set; }
    }

    public class CourseSportsFacilityDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<SportFacilityDto>? SportFacilities { get; set; }
    }

    public class SportFacilityDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class MunicipalityProcedureDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
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
        public JustMunicipalitysDto? Municipality { get; set; }
    }

    public class SocialMediaTypeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class PaymentHistoryDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public JustMunicipalitysDto? Municipality { get; set; }
        public MunicipalityProcedureDto? MunicipalityProcedures { get; set; }
        public AvailibityDto? Availability { get; set; }


    }

    public class AvailibityDto
    {
        public int Id { get; set; }
        public string? TypeStatus { get; set; }
    }
}