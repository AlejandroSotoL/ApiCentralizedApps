
namespace CentralizedApps.Models.Entities;

public partial class Course
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Get { get; set; }

    public string? Post { get; set; }

    public int? MunicipalityId { get; set; }

    public virtual Municipality? Municipality { get; set; }
}
