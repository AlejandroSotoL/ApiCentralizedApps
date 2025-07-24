using System;
using System.Collections.Generic;

namespace CentralizedApps.Domain.Entities;

public partial class SocialMediaType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<MunicipalitySocialMedium> MunicipalitySocialMedia { get; set; } = new List<MunicipalitySocialMedium>();
}
