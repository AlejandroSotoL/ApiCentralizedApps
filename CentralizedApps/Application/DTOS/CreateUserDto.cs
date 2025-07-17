using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CentralizedApps.Application.DTOS
{
    public class CreateUserDto
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; }
        public string NationalId { get; set; } = null!;
        public int DocumentTypeId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public long? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool LoginStatus { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}