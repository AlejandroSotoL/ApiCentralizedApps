using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CentralizedApps.Models.Entities;

public partial class DocumentType
{
    public int Id { get; set; }

    public string? NameDocument { get; set; }
    
    [JsonIgnore] 
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
