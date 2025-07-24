using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ValidationResponseDto> CreateUserAsync(CreateUserDto dto)
    {
        try
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
                LoginStatus = 2
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                BooleanStatus = true,
                CodeStatus = 200,
                SentencesError = ""
            };
        }
        catch (Exception e)
        {
            return new ValidationResponseDto
            {
                BooleanStatus = false,
                CodeStatus = 500,
                SentencesError = $"Error interno: {e.Message}"
            };
        }
    }

    public async Task<ValidationResponseDto> UpdateUserAsync(int id, CreateUserDto dto)
{
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null)
    {
        return new ValidationResponseDto
        {
            BooleanStatus = false,
            CodeStatus = 404,
            SentencesError = "User not found"
        };
    }

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
    user.LoginStatus = 2;

    _userRepository.Update(user);
    await _unitOfWork.SaveChangesAsync();

    return new ValidationResponseDto
    {
        BooleanStatus = true,
        CodeStatus = 200,
        SentencesError = ""
    };
}

}