using System;
using System.Collections.Generic;

namespace CentralizedApps.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? SecondLastName { get; set; }

    public string? NationalId { get; set; }

    public int? DocumentTypeId { get; set; }


    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? BirthDate { get; set; }

    public int? LoginStatus { get; set; }

    public virtual DocumentType? DocumentType { get; set; }

    public virtual Availability? LoginStatusNavigation { get; set; }

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();
}
