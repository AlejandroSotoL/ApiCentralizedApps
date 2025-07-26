using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services;

public class UserService : IUserService
{

    private readonly IUnitOfWork _unitOfWork;

    public UserService( IUnitOfWork unitOfWork)
    {
        
        _unitOfWork = unitOfWork;
    }

    public async Task<User> CreateUserAsync(CreateUserDto dto)
    {
        
            var user = new User
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                SecondLastName = dto.SecondLastName,
                NationalId = dto.NationalId,
                DocumentTypeId = dto.DocumentTypeId,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                BirthDate = dto.BirthDate,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                LoginStatus = false
            };
        _unitOfWork.UserRepository.AddAsync(user);

        return user;

                
    }

    public void UpdateUserAsync(User user, CreateUserDto dto)
{
    
    user.FirstName = dto.FirstName;
    user.MiddleName = dto.MiddleName;
    user.LastName = dto.LastName;
    user.SecondLastName = dto.SecondLastName;
    user.NationalId = dto.NationalId;
    user.DocumentTypeId = dto.DocumentTypeId;
    user.Email = dto.Email;
    user.BirthDate = dto.BirthDate;
    user.Password = dto.Password;
    user.PhoneNumber = dto.PhoneNumber;
    user.Address = dto.Address;
    user.LoginStatus = false;

_unitOfWork.UserRepository.Update(user);
    
}

}