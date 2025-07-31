using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos
{
    public class MunicipalityProcedureAddDto
    {
        public int? MunicipalityId { get; set; }

        public int? ProceduresId { get; set; }

        public string? IntegrationType { get; set; }

        public bool? IsActive { get; set; }
    }
}