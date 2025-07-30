
namespace CentralizedApps.Models.Entities;

public partial class Department
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
}
