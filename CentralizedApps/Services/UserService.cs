using CentralizedApps.Data;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.UserDtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Services.Interfaces;

namespace CentralizedApps.Services;

public class UserService : IUserService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly CentralizedAppsDbContext _context;

    private readonly IPasswordService _passwordService;
    public UserService(IPasswordService passwordService, IUnitOfWork unitOfWork, CentralizedAppsDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _passwordService = passwordService;
    }

    public async Task<ValidationResponseDto> ChangeStatusUser(int userId, bool? status)
    {
        try
        {
            var user = await _unitOfWork.genericRepository<User>().FindAsync_Predicate(u => u.Id == userId);
            if (user == null || status == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Usuario no encontrado o estado inválido"
                };
            }

            user.LoginStatus = status.Value;
            _unitOfWork.genericRepository<User>().Update(user);
            var rowsAffected = await _unitOfWork.SaveChangesAsync();
            if (rowsAffected > 0)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Muchas Gracias por visitarnos..."
                };
            }
            else
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error al actualizar el estado del usuario"
                };
            }
        }
        catch (Exception e)
        {
            return new ValidationResponseDto
            {
                BooleanStatus = false,
                CodeStatus = 400,
                SentencesError = "Error: no se pudo cambiar el estado del usuario ERROR: " + e.Message
            };
        }
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
    public async Task<ValidationResponseDto> UpdatePasswordUser(int userId, UpdatePasswordRequestDto updatePasswordRequestDto)
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

            bool isValid = BCrypt.Net.BCrypt.Verify(currentlyUser.Password, updatePasswordRequestDto.CurrentPassword);
            if (!isValid) {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    SentencesError = "Contraseña actual es incorrecta"
                };
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(updatePasswordRequestDto.NewPassword);
            currentlyUser.Password = hashedPassword;
            _unitOfWork.genericRepository<User>().Update(currentlyUser);
            var rows = await _unitOfWork.SaveChangesAsync();
            if (rows > 0)
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

    public async Task<ValidationResponseDto> UpdatePasswordByForget(int userId, UpdatePasswordByForget request)
    {
        try
        {
            var user = await _unitOfWork.genericRepository<User>().GetByIdAsync(userId);
            if (user == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Usuario no encontrado"
                };
            }


            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _unitOfWork.genericRepository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new ValidationResponseDto
            {
                BooleanStatus = true,
                CodeStatus = 200,
                SentencesError = "Contraseña actualizada correctamente"
            };
        }
        catch (Exception e)
        {
            return new ValidationResponseDto
            {
                BooleanStatus = false,
                CodeStatus = 500,
                SentencesError = "Error al actualizar la contraseña: " + e.Message
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
        user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
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

    public async Task<ValidationResponseDto> DeleteUser(int id)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = "Usuario no encontrado"
                };
            }

            try
            {
                var reminders = await _unitOfWork.genericRepository<Reminder>()
                    .GetAllWithFilterAsync(r => r.IdUser == id);
                if (reminders.Any())
                    _unitOfWork.genericRepository<Reminder>().DeleteRange(reminders);
            }
            catch (Exception exReminders)
            {
                await transaction.RollbackAsync();
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error eliminando recordatorios: {exReminders.Message}"
                };
            }


            try
            {
                var payments = await _unitOfWork.paymentHistoryRepository
                    .GetAllWithFilterAsync(p => p.UserId == id);
                if (payments.Any())
                    _unitOfWork.paymentHistoryRepository.DeleteRange(payments);
            }
            catch (Exception exPayments)
            {
                await transaction.RollbackAsync();
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error eliminando historiales de pago: {exPayments.Message}"
                };
            }

            try
            {
                _unitOfWork.UserRepository.Delete(user);
            }
            catch (Exception exUser)
            {
                await transaction.RollbackAsync();
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error eliminando usuario: {exUser.Message}"
                };
            }

            await _unitOfWork.CompleteAsync();
            await transaction.CommitAsync();

            return new ValidationResponseDto
            {
                BooleanStatus = true,
                CodeStatus = 200,
                SentencesError = $"{user.FirstName} {user.LastName} y todos sus registros asociados han sido eliminados correctamente."
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ValidationResponseDto
            {
                BooleanStatus = false,
                CodeStatus = 500,
                SentencesError = $"Ocurrió un error inesperado: {ex.Message}"
            };
        }
    }
}