using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Enums;
using VetClinicCRM.Core.Models;


namespace VetClinicCRM.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // Специфичные методы для пользователей
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<User> AuthenticateAsync(string username, string password);
        Task ChangePasswordAsync(int userId, string newPassword);
        Task DeactivateUserAsync(int userId);
        Task<IEnumerable<User>> GetActiveUsersAsync();
    }
}
