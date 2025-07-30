
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserDto dto);
        void UpdateUserAsync(User user, UserDto dto);
    }
}