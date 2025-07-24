using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class Municipality
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? EntityCode { get; set; }

    public int? DepartmentId { get; set; }

    public int? ThemeId { get; set; }

    public int? IsActive { get; set; }

    public string? Domain { get; set; }

    public virtual ICollection<CourseSportsFacility> CourseSportsFacilities { get; set; } = new List<CourseSportsFacility>();

    public virtual Department? Department { get; set; }

    public virtual Availability? IsActiveNavigation { get; set; }

    public virtual ICollection<MunicipalityProcedure> MunicipalityProcedures { get; set; } = new List<MunicipalityProcedure>();

    public virtual ICollection<MunicipalitySocialMedium> MunicipalitySocialMedia { get; set; } = new List<MunicipalitySocialMedium>();

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();

    public virtual ICollection<QueryField> QueryFields { get; set; } = new List<QueryField>();

    public virtual Theme? Theme { get; set; }
}
