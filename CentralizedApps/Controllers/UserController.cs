
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
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("user not found");

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var response = await _userService.CreateUserAsync(dto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateUserDto userUpdated)
        {
            var result = await _userService.UpdateUserAsync(id, userUpdated);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found");

            var response = _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return Ok(response);
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
