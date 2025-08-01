using CentralizedApps.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Repositories.Interfaces;

namespace CentralizedApps.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found");

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return Ok(new ValidationResponseDto
            {
                BooleanStatus = true,
                CodeStatus = 200,
                SentencesError = "delete user succcesfully"
            });
        }

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetByEmailUser(string email)
        {
            try
            {
                var response = await _unitOfWork.UserRepository.GetByEmailUserByAuthenticate(email);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 404,
                    SentencesError = e.Message
                });
            }
        }

    }
}
