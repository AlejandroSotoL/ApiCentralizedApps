

namespace CentralizedApps.Models.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; }
        public string NationalId { get; set; } = null!;
        public int DocumentTypeId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int LoginStatus { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}