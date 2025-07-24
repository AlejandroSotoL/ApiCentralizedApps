using System;
using System.Collections.Generic;

namespace CentralizedApps.Domain.Entities;

public partial class SportsFacility
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Get { get; set; }

    public string? CalendaryPost { get; set; }

    public string? ReservationPost { get; set; }

    public virtual ICollection<CourseSportsFacility> CourseSportsFacilities { get; set; } = new List<CourseSportsFacility>();
}
