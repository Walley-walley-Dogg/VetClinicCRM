using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Models;

namespace VetClinicCRM.Core.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        // Специфичные методы для клиентов
        Task<Client> GetByPhoneAsync(string phoneNumber);
        Task<Client> GetByEmailAsync(string email);
        Task<IEnumerable<Client>> SearchAsync(string searchTerm);
        Task<IEnumerable<Client>> GetWithPetsAsync();
        Task<Client> GetWithPetsAsync(int id);
        Task<int> GetTotalCountAsync();
    }
}
