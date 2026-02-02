using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Enums;
using VetClinicCRM.Core.Interfaces;
using VetClinicCRM.Core.Models;
using VetClinicCRM.Data.Data;
using static BCrypt.Net.BCrypt;

namespace VetClinicCRM.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            return await _dbSet
                .Where(u => u.Role == role && u.IsActive)
                .ToListAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet
                .AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email);
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await GetByUsernameAsync(username);

            if (user == null || !user.IsActive)
                return null;

            // Проверяем пароль
            var isValidPassword = Verify(password, user.PasswordHash);

            if (!isValidPassword)
                return null;

            // Обновляем дату последнего входа
            user.LastLoginDate = DateTime.Now;
            await UpdateAsync(user);

            return user;
        }

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.PasswordHash = HashPassword(newPassword);
                await UpdateAsync(user);
            }
        }

        public async Task DeactivateUserAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                await UpdateAsync(user);
            }
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet
                .Where(u => u.IsActive)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        // Переопределяем метод AddAsync для хеширования пароля
        public override async Task<User> AddAsync(User entity)
        {
            if (!string.IsNullOrEmpty(entity.Password))
            {
                entity.PasswordHash = HashPassword(entity.Password);
                entity.Salt = GenerateSalt();
            }

            return await base.AddAsync(entity);
        }

        // Переопределяем метод UpdateAsync для обработки пароля
        public override async Task UpdateAsync(User entity)
        {
            // Если при обновлении передали новый пароль
            if (!string.IsNullOrEmpty(entity.Password))
            {
                entity.PasswordHash = HashPassword(entity.Password);
                entity.Salt = GenerateSalt();
            }

            await base.UpdateAsync(entity);
        }
    }
}

