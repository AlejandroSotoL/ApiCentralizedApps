using System;
using System.Collections.Generic;

namespace CentralizedApps.Models.Entities;

public partial class PeopleInvitated
{
    public string? DocumentationDni { get; set; }

    public string CompleteName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public int Id { get; set; }
}
