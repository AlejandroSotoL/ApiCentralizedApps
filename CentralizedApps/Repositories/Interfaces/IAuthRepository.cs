

using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Repositories.Interfaces

{
    public interface IAuthRepository
    {
        Task<ValidationResponseDto> Login(string Email , string Password);
    }
}