
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;

namespace CentralizedApps.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserDto dto);
        Task<ValidationResponseDto> UpdateUserAsync(int id, UserDto updateUserDto);
        Task<ValidationResponseDto> UpdatePasswordUser(int userId, string CurrentlyPassword, string NewPassword);
        Task<ValidationResponseDto>ChangeStatusUser(int userId, bool ?status);
    }
}