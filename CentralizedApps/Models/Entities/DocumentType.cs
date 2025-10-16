using System.Text.Json.Serialization;

namespace CentralizedApps.Models.Entities;

public partial class DocumentType
{
    public int Id { get; set; }

    public string? NameDocument { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
