using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos
{
    public class CreateReminderDto
    {
        public int? IdProcedureMunicipality { get; set; }
        public int? IdUser { get; set; }
        public DateOnly? ExpirationDate { get; set; }

        public string? VigenciaDate { get; set; }

        public string? ReminderType { get; set; }

    }
}