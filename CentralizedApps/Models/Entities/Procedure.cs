
namespace CentralizedApps.Models.Entities;

public partial class Procedure
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MunicipalityProcedure> MunicipalityProcedures { get; set; } = new List<MunicipalityProcedure>();
}
