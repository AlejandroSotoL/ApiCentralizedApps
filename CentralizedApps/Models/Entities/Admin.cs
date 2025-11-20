using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class Admin
{
    public int Id { get; set; }

    public string CompleteName { get; set; } = null!;

    public string PasswordAdmin { get; set; } = null!;

    public string UserNanem { get; set; } = null!;

    public int? IdRol { get; set; }

    public virtual Role? IdRolNavigation { get; set; }
}
