using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Application.DTOS;
using CentralizedApps.Domain.Entities;
using CentralizedApps.Domain.Interfaces;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedApps.Presentation.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var users = await _unitOfWork.genericRepository<User>().GetAllAsync();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _unitOfWork.genericRepository<User>().GetByIdAsync(id);
            if (user == null)
                NotFound("user not fund"); // 404

            return Ok(user); // 200
        }

        // POST: api/User
    [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                SecondLastName = dto.SecondLastName,
                NationalId = dto.NationalId,
                DocumentTypeId = dto.DocumentTypeId,
                Email = dto.Email,
                Password = dto.Password, 
                BirthDate = dto.BirthDate,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                LoginStatus = dto.LoginStatus
            };

            var response = await _unitOfWork.genericRepository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(response);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> updateUser(int id, [FromBody] CreateUserDto userUpdated)
        {
            var user = await _unitOfWork.genericRepository<User>().GetByIdAsync(id);
            if (user == null)
            {
                
                return NotFound("User not fund");   // 404
            }

                
                user.FirstName = userUpdated.FirstName;
                user.MiddleName = userUpdated.MiddleName;
                user.LastName = userUpdated.LastName;
                user.SecondLastName = userUpdated.SecondLastName;
                user.NationalId = userUpdated.NationalId;
                user.DocumentTypeId = userUpdated.DocumentTypeId;
                user.Email = userUpdated.Email;
                user.BirthDate = userUpdated.BirthDate;
                user.Password = userUpdated.Password;
                user.PhoneNumber = userUpdated.PhoneNumber;
                user.Address = userUpdated.Address;
                user.LoginStatus = userUpdated.LoginStatus;


        var response = _unitOfWork.genericRepository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(response); // 200 
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            var user = await _unitOfWork.genericRepository<User>().GetByIdAsync(id);
            if (user == null)
                return NotFound("User not fund"); //404

        var response = _unitOfWork.genericRepository<User>().Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(response); // 200 
        }
    }



}
