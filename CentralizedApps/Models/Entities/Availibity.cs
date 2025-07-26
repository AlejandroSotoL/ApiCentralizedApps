using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class Availibity
{
    public int Id { get; set; }

    public string? TypeStatus { get; set; }

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();
}
