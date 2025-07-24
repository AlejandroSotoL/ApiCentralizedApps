using System;
using System.Collections.Generic;

namespace CentralizedApps.Domain.Entities;

public partial class MunicipalitySocialMedium
{
    public int Id { get; set; }

    public int? MunicipalityId { get; set; }

    public int? SocialMediaTypeId { get; set; }

    public string? Url { get; set; }

    public int? IsActive { get; set; }

    public virtual Availability? IsActiveNavigation { get; set; }

    public virtual Municipality? Municipality { get; set; }

    public virtual SocialMediaType? SocialMediaType { get; set; }
}
