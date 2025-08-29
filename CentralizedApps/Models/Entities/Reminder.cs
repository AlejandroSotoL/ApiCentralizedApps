using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class Reminder
{
    public int Id { get; set; }

    public int? IdProcedureMunicipality { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public string? VigenciaDate { get; set; }

    public string? ReminderType { get; set; }

    public int? IdUser { get; set; }

    public virtual MunicipalityProcedure? IdProcedureMunicipalityNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
