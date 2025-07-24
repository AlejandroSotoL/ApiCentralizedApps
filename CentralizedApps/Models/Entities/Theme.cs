using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class Theme
{
    public int Id { get; set; }

    public string? BackGroundColor { get; set; }

    public string? Shield { get; set; }

    public string? PrimaryColor { get; set; }

    public string? SecondaryColor { get; set; }

    public string? SecondaryColorBlack { get; set; }

    public string? OnPrimaryColorLight { get; set; }

    public string? OnPrimaryColorDark { get; set; }

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
}
