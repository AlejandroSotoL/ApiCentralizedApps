using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class ShieldMunicipality
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public string? NameOfMunicipality { get; set; }

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
}
