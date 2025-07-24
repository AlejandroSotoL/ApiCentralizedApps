using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class MunicipalityProcedure
{
    public int Id { get; set; }

    public int? MunicipalityId { get; set; }

    public int? ProceduresId { get; set; }

    public string? IntegrationType { get; set; }

    public int? IsActive { get; set; }

    public virtual Availability? IsActiveNavigation { get; set; }

    public virtual Municipality? Municipality { get; set; }

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();

    public virtual Procedure? Procedures { get; set; }
}
