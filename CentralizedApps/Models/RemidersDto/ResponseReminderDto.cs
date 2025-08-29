using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Dtos.PrincipalsDtos;

namespace CentralizedApps.Models.RemidersDto
{
    public class ResponseReminderDto
    {
        public DateOnly? ExpirationDate { get; set; }
        public string? VigenciaDate { get; set; }
        public string? ReminderType { get; set; }
        public MunicipalityProcedureDto? IdProcedureMunicipalityNavigation { get; set; }
        public UserDto_Munucipality? IdUserNavigation { get; set; }
    }
}