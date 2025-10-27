using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class Role
{
    public int Id { get; set; }

    public string? TypeRole { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();
}
