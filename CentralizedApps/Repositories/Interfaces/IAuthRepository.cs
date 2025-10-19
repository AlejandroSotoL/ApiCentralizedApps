using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Repositories.Interfaces

{
    public interface IAuthRepository
    {
        Task<ValidationResponseDto> Login(string Email, string Password);
        Task<Admin> LoginAdmins(string Username, string Password);
        Task<bool> AddAdmin(string completeName, string username, string convertPASSWORDTO);
    }
}