using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Models.Dtos
{
    public class CreatePeopleInvitated
    {
        public string DocumentationDni { get; set; }

        public string CompleteName { get; set; } = null!;

        public string PhoneNumber { get; set; }

        public string Email { get; set; } = null!;
    }
}