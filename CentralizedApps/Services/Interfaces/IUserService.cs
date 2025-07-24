
using CentralizedApps.Models.Dtos;

namespace CentralizedApps.Services.Interfaces
{
    public interface IUserService
    {
        Task<ValidationResponseDto> CreateUserAsync(CreateUserDto dto);
        Task<ValidationResponseDto> UpdateUserAsync(int id, CreateUserDto dto);
    }
}