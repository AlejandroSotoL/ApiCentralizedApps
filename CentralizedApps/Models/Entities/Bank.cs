using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class Bank
{
    public int Id { get; set; }

    public string? NameBank { get; set; }

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
}
