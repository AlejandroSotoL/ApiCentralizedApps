using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos
{
    public class CourseSportsFacilityDto
    {
        public int? MunicipalityId { get; set; }

        public CourseDto? courseDto { get; set; }

        public SportsFacilityDto? sportsFacilityDto { get; set; }
    }
}