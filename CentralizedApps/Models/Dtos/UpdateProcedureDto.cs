
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Models.Dtos
{
    public class UpdateProcedureDto
    {
        public int? Id { get; set; }
        public int? MunicipalityId { get; set; }

        // CAMBIO: Usamos una clase simple, no la Entidad de BD para evitar errores de validación (Name required)
        public ProcedureIdDto? Procedures { get; set; }

        public string? IntegrationType { get; set; }
        public bool? IsActive { get; set; }
    }

    // Clase auxiliar pequeña para mapear el JSON { "Id": X }
    public class ProcedureIdDto
    {
        public int Id { get; set; }
    }
}