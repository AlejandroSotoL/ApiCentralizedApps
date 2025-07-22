using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CentralizedApps.Application.DTOS;
using CentralizedApps.Domain.Entities;
using CentralizedApps.Domain.Interfaces;
using CentralizedApps.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentralizedApps.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly CentralizedAppsDbContext _Context;
        private readonly DbSet<T> _DBset;

        public GenericRepository(CentralizedAppsDbContext Context)
        {
            _Context = Context;
            _DBset = _Context.Set<T>();
        }

        public async Task<string> AddAsync(T entity)
        {
            try
            {
                await _DBset.AddAsync(entity);
                return "User created successfully";
            }
            catch (System.Exception ex)
            {
                return $"invalid data {ex.Message}";
            }
        }

        public string Delete(T entity)
        {
            try
            {
                _DBset.Remove(entity);

                return " User delete sucessfully";
            }
            catch (System.Exception ex)
            {
                return $"invalid data {ex.Message}";
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _DBset.ToListAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error getting data: {ex.Message}");
                return new List<T>();
            }
        }

        public async Task<User?> GetByEmailUserByAuthenticate(string email)
        {
            try
            {
                return await _Context.Users
                    .FirstOrDefaultAsync(x => x.Email == email);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _DBset.FindAsync(id);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error getting data: {ex.Message}");
                return null;
            }
        }

        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> filter)
        {
            return await _DBset.FindAsync(filter);
        }

        public string Update(T entity)
        {
            try
            {
                _DBset.Update(entity);

                return "Uodate sucessfullly";
            }
            catch (System.Exception ex)
            {
                return $"error updating data: {ex.Message}";
            }
        }
    }
}