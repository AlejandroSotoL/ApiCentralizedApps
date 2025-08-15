using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services;

public class UserService : IUserService
{

    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {

        _unitOfWork = unitOfWork;
    }

    public async Task<User> CreateUserAsync(UserDto dto)
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
        await _unitOfWork.genericRepository<User>().AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user;


    }

    public async Task<ValidationResponseDto> UpdatePasswordUser(int userId, string CurrentlyPassword, string NewPassword)
    {
        try
        {
            var currentlyUser = await _unitOfWork.genericRepository<User>().GetByIdAsync(userId);
            if (currentlyUser == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Usuario no encontrado"
                };
            }

            //validation password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(CurrentlyPassword, currentlyUser.Password);
            if (!isPasswordValid)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "La Contraseña actual es incorrecta"
                };
            }

            var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            currentlyUser.Password = hashedNewPassword;
            _unitOfWork.genericRepository<User>().Update(currentlyUser);
            var rowws = await _unitOfWork.SaveChangesAsync();
            if (rowws > 0 && isPasswordValid)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Contraseña actualizada correctamente"
                };
            }
            else
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error al actualizar la contraseña"
                };
            }
        }
        catch (Exception e)
        {
            return new ValidationResponseDto
            {
                BooleanStatus = false,
                CodeStatus = 600,
                SentencesError = "Error al actualizar la contraseña: Error -> " + e.Message
            };
        }
    }

    public async Task<ValidationResponseDto> UpdateUserAsync(int id, UserDto updateUserDto)
    {
        var user = await _unitOfWork.genericRepository<User>().GetByIdAsync(id);
        if (user == null)
        {
            return new ValidationResponseDto
            {
                BooleanStatus = false,
                CodeStatus = 400,
                SentencesError = "notfund"
            };
        }
        user.FirstName = updateUserDto.FirstName;
        user.MiddleName = updateUserDto.MiddleName;
        user.LastName = updateUserDto.LastName;
        user.SecondLastName = updateUserDto.SecondLastName;
        user.NationalId = updateUserDto.NationalId;
        user.DocumentTypeId = updateUserDto.DocumentTypeId;
        user.Email = updateUserDto.Email;
        user.BirthDate = updateUserDto.BirthDate;
        user.Password = updateUserDto.Password;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.Address = updateUserDto.Address;
        user.LoginStatus = false;

        _unitOfWork.genericRepository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();
        return new ValidationResponseDto
        {
            CodeStatus = 200,
            BooleanStatus = true,
            SentencesError = "succesfully"
        };

    }

}