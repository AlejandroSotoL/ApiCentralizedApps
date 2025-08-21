
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.UserDtos;

namespace CentralizedApps.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserDto dto);
        Task<ValidationResponseDto> UpdateUserAsync(int id, UserDto updateUserDto);
        Task<ValidationResponseDto> UpdatePasswordUser(int userId, UpdatePasswordRequestDto updatePasswordRequestDto);
        Task<ValidationResponseDto>ChangeStatusUser(int userId, bool ?status);
    }
}