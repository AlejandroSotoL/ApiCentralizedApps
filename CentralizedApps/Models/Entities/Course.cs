using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class Course
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Get { get; set; }

    public string? Post { get; set; }

    public virtual ICollection<CourseSportsFacility> CourseSportsFacilities { get; set; } = new List<CourseSportsFacility>();
}
