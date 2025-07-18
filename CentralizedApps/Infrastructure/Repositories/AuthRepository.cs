using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CentralizedApps.Domain.Entities;
using CentralizedApps.Domain.Interfaces;
using CentralizedApps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly GenericRepository<User> _GenericRepository;
        private readonly CentralizedAppsDbContext _Context;
        public AuthRepository( CentralizedAppsDbContext context)
        {
            _Context = context;
        }
        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var user = await _Context.Users
                    .FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
                return user != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}