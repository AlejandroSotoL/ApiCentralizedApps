﻿using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class MunicipalitySocialMedium
{
    public int Id { get; set; }

    public int? MunicipalityId { get; set; }

    public int? SocialMediaTypeId { get; set; }

    public string? Url { get; set; }

    public bool? IsActive { get; set; }

    public virtual Municipality? Municipality { get; set; }

    public virtual SocialMediaType? SocialMediaType { get; set; }
}
