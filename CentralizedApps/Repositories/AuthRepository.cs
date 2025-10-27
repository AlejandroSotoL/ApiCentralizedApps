
using CentralizedApps.Data;
using CentralizedApps.Models.Dtos;
using CentralizedApps.Models.Entities;
using CentralizedApps.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CentralizedAppsDbContext _Context;
        public AuthRepository(CentralizedAppsDbContext context)
        {
            _Context = context;
        }
        public async Task<ValidationResponseDto> Login(string email, string password)
        {
            try
            {
                var user = await _Context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user is null)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 404,
                        SentencesError = "El usuario no existe"
                    };
                }

                bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (!passwordValid)
                {
                    return new ValidationResponseDto
                    {
                        BooleanStatus = false,
                        CodeStatus = 401,
                        SentencesError = "Contraseña incorrecta"
                    };
                }

                user.LoginStatus = true;
                await _Context.SaveChangesAsync();

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Inicio de sesión exitoso"
                };
            }
            catch (Exception ex)
            {
                return new ValidationResponseDto
                {
                    BooleanStatus = false,
                    CodeStatus = 500,
                    SentencesError = $"Error interno: {ex.Message}"
                };
            }
        }

        public async Task<Admin> LoginAdmins(string Username, string Password)
        {
            try
            {
                Console.WriteLine(Username);
                var user = await _Context.Admins
                    .Include(x => x.IdRolNavigation)
                    .FirstOrDefaultAsync(u => u.UserNanem == Username);

                if (user is null)
                {
                    Console.WriteLine("Es null");
                    return null;
                }

                //bool passwordValid = BCrypt.Net.BCrypt.Verify(Password, user.PasswordAdmin);
                //if (!passwordValid)
                //{
                //    return null;
                //}

                Console.WriteLine(user.CompleteName);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> AddAdmin(string completeName, string username, string convertPASSWORDTO)
        {
            try
            {
                var newAdmin = new Admin
                {
                    CompleteName = completeName,
                    UserNanem = username,
                    PasswordAdmin = convertPASSWORDTO,
                    IdRol = 1
                };

                await _Context.Admins.AddAsync(newAdmin);
                await _Context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}