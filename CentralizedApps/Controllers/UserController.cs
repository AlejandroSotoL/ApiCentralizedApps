
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

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {

            try
            {
                await _userService.CreateUserAsync(dto);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = ""
                });
            }
            catch (Exception ex)
            {
                return Ok(new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 400,
                    SentencesError = $"Error: {ex.Message}"
                });
            }
        
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateUserDto userUpdated)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound(new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 400,
                        SentencesError = $"Error: not found"
                    });
                
                _userService.UpdateUserAsync(user, userUpdated);
                await _unitOfWork.SaveChangesAsync();

            return Ok(new ValidationResponseDto
            {
                BooleanStatus = true,
                CodeStatus = 200,
                SentencesError = ""
            });
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found");

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return  Ok(new ValidationResponseDto
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
