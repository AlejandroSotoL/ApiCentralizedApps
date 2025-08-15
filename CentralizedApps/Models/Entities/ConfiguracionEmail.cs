using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class ConfiguracionEmail
{
    public int Id { get; set; }

    public string Recurso { get; set; } = null!;

    public string Propiedad { get; set; } = null!;

    public string Valor { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
