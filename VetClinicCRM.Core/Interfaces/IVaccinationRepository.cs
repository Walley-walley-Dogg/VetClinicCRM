using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Models;

namespace VetClinicCRM.Core.Interfaces
{
    public interface IVaccinationRepository : IRepository<Vaccination>
    {
        // Специфичные методы для вакцинаций
        Task<IEnumerable<Vaccination>> GetByPetIdAsync(int petId);
        Task<IEnumerable<Vaccination>> GetUpcomingVaccinationsAsync(int daysAhead);
        Task<IEnumerable<Vaccination>> GetOverdueVaccinationsAsync();
        Task<IEnumerable<Vaccination>> GetVaccinationsDueAsync(DateTime date);
        Task<Vaccination> GetWithPetAsync(int id);
        Task<IEnumerable<Vaccination>> GetByVaccineNameAsync(string vaccineName);
        Task<bool> HasVaccinationAsync(int petId, string vaccineName, DateTime date);
    }
}

