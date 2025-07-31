using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos
{
    public class CreateMunicipalitySocialMediumDto
    {
        public int? MunicipalityId { get; set; }

        public int? SocialMediaTypeId { get; set; }

        public string? Url { get; set; }

        public bool? IsActive { get; set; }
    }
}