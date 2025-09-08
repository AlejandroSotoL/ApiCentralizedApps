using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;
using CentralizedApps.Models.Entities;
using CentralizedApps.Models.UserDtos;
using System.ComponentModel.DataAnnotations;
using CentralizedApps.Data;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRemidersService _remidersService;

        private readonly IUnitOfWork _unitOfWork;
        private readonly CentralizedAppsDbContext _context;
        private readonly IPasswordService _passwordService;

        public UserController(IPasswordService passwordService, IUserService userService, IUnitOfWork unitOfWork, CentralizedAppsDbContext context, IRemidersService remidersService)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _context = context;
            _remidersService = remidersService;
            _passwordService = passwordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetAllAsync();
                return Ok(users);

            }
            catch
            {
                return NotFound(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = $"Error: not found"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,       
                    SentencesError = $"Error: not found"
                });
            user.Password = _passwordService.Decrypt(user.Password);
            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<ValidationResponseDto> CreateUser(UserDto userdto)
        {

            try
            {
                await _userService.CreateUserAsync(userdto);
                await _unitOfWork.SaveChangesAsync();

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "OKKKK"
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = $"Error: {ex.Message}"
                };
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userUpdated)
        {
            try
            {

                if (userUpdated == null)
                    return BadRequest(new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = $"el objeto no puede ser null"
                    });

                var result = await _userService.UpdateUserAsync(id, userUpdated);
                if (!result.BooleanStatus)
                {
                    return BadRequest(new ValidationResponseDto
                    {
                        BooleanStatus = result.BooleanStatus,
                        CodeStatus = result.CodeStatus,
                        SentencesError = "Error: " + result.SentencesError
                    });
                }
                else
                {
                    return Ok(new ValidationResponseDto
                    {
                        BooleanStatus = result.BooleanStatus,
                        CodeStatus = result.CodeStatus,
                        SentencesError = result.SentencesError
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = ex.Message
                });
            }

        }

        [HttpDelete("Delete/{id}")]
        public async Task<ValidationResponseDto> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                return result;
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Ocurrió un error inesperado eliminando el usuario: {ex.Message}"
                };
            }
        }



        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmailUser([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ValidationResponseDto
                {
                    CodeStatus = 400,
                    BooleanStatus = false,
                    SentencesError = "El email no puede estar vacío"
                });
            }

            var response = await _unitOfWork.UserRepository.GetByEmailUserByAuthenticate(email);

            if (response == null)
            {
                return NotFound(new ValidationResponseDto
                {
                    CodeStatus = 404,
                    BooleanStatus = false,
                    SentencesError = "Usuario no encontrado"
                });
            }
            response.Password = _passwordService.Decrypt(response.Password);
            return Ok(response);
        }

        [HttpPut("update-password/{userId}")]
        public async Task<ValidationResponseDto> UpdatePasswordUser(int userId, [FromBody] UpdatePasswordRequestDto updatePasswordRequestDto)
        {
            try
            {
                var response = await _userService.UpdatePasswordUser(userId, updatePasswordRequestDto);
                if (response.BooleanStatus == false)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = response.SentencesError
                    };
                }
                else
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = true,
                        CodeStatus = 200,
                        SentencesError = $"{response.SentencesError} - por favor iniciar sesión nuevamente"
                    };
                }
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error: no se pudo actualizar la contraseña ERROR: " + e.Message
                };
            }
        }


        [HttpPut("updatePasswordByForget/{userId}")]
        public async Task<ValidationResponseDto> UpdatePasswordByForget(int userId, [FromBody] UpdatePasswordByForget request)
        {
            try
            {
                var response = await _userService.UpdatePasswordByForget(userId, request);
                return new ValidationResponseDto
                {
                    BooleanStatus = response.BooleanStatus,
                    CodeStatus = response.CodeStatus,
                    SentencesError = response.SentencesError
                };
            }
            catch (Exception e)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = "Error: no se pudo actualizar la contraseña ERROR: " + e.Message
                };
            }
        }

        [HttpPut("ChangeStatusUser/{userId}/status/{status}")]
        public async Task<ValidationResponseDto> ChangeStatusUser(int userId, bool status)
        {
            try
            {
                var response = await _userService.ChangeStatusUser(userId, status);
                return new ValidationResponseDto
                {
                    BooleanStatus = response.BooleanStatus,
                    CodeStatus = response.CodeStatus,
                    SentencesError = response.SentencesError
                };
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
    }
}
