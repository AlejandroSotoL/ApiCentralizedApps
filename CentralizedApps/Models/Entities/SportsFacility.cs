using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class SportsFacility
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Get { get; set; }

    public string? CalendaryPost { get; set; }

    public string? ReservationPost { get; set; }

    public int? MunicipalityId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Municipality? Municipality { get; set; }
}
