
namespace CentralizedApps.Models.Entities;

public partial class CourseSportsFacility
{
    public int Id { get; set; }

    public int? SportFacilitiesId { get; set; }

    public int? CoursesId { get; set; }

    public int? MunicipalityId { get; set; }

    public virtual Course? Courses { get; set; }

    public virtual Municipality? Municipality { get; set; }

    public virtual SportsFacility? SportFacilities { get; set; }
}
