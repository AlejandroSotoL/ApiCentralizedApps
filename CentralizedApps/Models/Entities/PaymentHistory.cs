using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class PaymentHistory
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public bool? Status { get; set; }

    public int? MunicipalityProceduresId { get; set; }

    public int? StatusType { get; set; }

    public virtual MunicipalityProcedure? MunicipalityProcedures { get; set; }

    public virtual Availibity? StatusTypeNavigation { get; set; }

    public virtual User? User { get; set; }
}
