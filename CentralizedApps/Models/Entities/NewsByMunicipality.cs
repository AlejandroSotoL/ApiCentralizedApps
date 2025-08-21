using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class NewsByMunicipality
{
    public int Id { get; set; }

    public int? IdMunicipality { get; set; }

    public string? GetUrlNew { get; set; }

    public virtual Municipality? IdMunicipalityNavigation { get; set; }
}
