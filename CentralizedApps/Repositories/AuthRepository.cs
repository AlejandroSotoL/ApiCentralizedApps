
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
                if (user != null)
                {
                user.LoginStatus = true ;
                    
                }
                
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
                await _Context.SaveChangesAsync();

                return new ValidationResponseDto
                {
                    BooleanStatus = true,
                    CodeStatus = 200,
                    SentencesError = "Inicio de sesión exitoso",
                    
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

    }
}