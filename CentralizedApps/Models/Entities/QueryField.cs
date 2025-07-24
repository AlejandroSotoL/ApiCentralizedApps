using System;
using System.Collections.Generic;

namespace CentralizedApps.Domain.Entities;

public partial class QueryField
{
    public int Id { get; set; }

    public int? MunicipalityId { get; set; }

    public string? FieldName { get; set; }

    public virtual Municipality? Municipality { get; set; }
}
