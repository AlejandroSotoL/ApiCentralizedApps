using System;
using System.Collections.Generic;

namespace CentralizedApps.Domain.Entities;

public partial class DocumentType
{
    public int Id { get; set; }

    public string? NameDocument { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
