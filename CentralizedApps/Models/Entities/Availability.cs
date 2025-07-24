
namespace CentralizedApps.Models.Entities;

public partial class Availability
{
    public int Id { get; set; }

    public bool? StatusType { get; set; }

    public virtual ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();

    public virtual ICollection<MunicipalityProcedure> MunicipalityProcedures { get; set; } = new List<MunicipalityProcedure>();

    public virtual ICollection<MunicipalitySocialMedium> MunicipalitySocialMedia { get; set; } = new List<MunicipalitySocialMedium>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
