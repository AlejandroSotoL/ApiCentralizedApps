using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.RemidersDto;

namespace CentralizedApps.Models.Dtos
{
    public class CreateMunicipalityProcedures_Web
    {
        public List<Procedure>? Procedures { get; set; }
        public List<MunicipalityProcedureDto_Reminders>? ProceduresWithRelations { get; set; }
        public List<Municipality>? Municipalities { get; set; }

    }
}