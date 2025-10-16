using CentralizedApps.Models.Dtos.PrincipalsDtos;

namespace CentralizedApps.Models.RemidersDto
{

    public class ResponseReminderDto
    {
        public int? Id { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? VigenciaDate { get; set; }
        public string? ReminderType { get; set; }
        public MunicipalityProcedureDto_Reminders? IdProcedureMunicipalityNavigation { get; set; }
        public UserDto_Munucipality? IdUserNavigation { get; set; }

    }

    public class MunicipalityProcedureDto_Reminders
    {
        public int Id { get; set; }
        public string IntegrationType { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public ProcedureDto? Procedures { get; set; }
        public JustMunicipalitysDto? Municipality { get; set; }

    }
}
