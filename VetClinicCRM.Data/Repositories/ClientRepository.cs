using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Interfaces;
using VetClinicCRM.Core.Models;
using VetClinicCRM.Data.Data;

namespace VetClinicCRM.Data.Interfaces
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Client> GetByPhoneAsync(string phoneNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Client>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            searchTerm = searchTerm.ToLower();
            return await _dbSet
                .Where(c => c.LastName.ToLower().Contains(searchTerm) ||
                           c.FirstName.ToLower().Contains(searchTerm) ||
                           c.Patronymic.ToLower().Contains(searchTerm) ||
                           c.PhoneNumber.Contains(searchTerm) ||
                           (c.Email != null && c.Email.ToLower().Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetWithPetsAsync()
        {
            return await _dbSet
                .Include(c => c.Pets)
                .ToListAsync();
        }

        public async Task<Client> GetWithPetsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Pets)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
