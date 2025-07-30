using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos
{
    public class QueryFieldDto
    {
        public int? MunicipalityId { get; set; }

        public string? FieldName { get; set; }
    }
}