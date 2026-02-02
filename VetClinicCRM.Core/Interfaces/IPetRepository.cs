using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Enums;
using VetClinicCRM.Core.Models;

namespace VetClinicCRM.Core.Interfaces
{
    public interface IPetRepository : IRepository<Pet>
    {
        // Специфичные методы для питомцев
        Task<IEnumerable<Pet>> GetByClientIdAsync(int clientId);
        Task<IEnumerable<Pet>> GetBySpeciesAsync(PetSpecies species);
        Task<Pet> GetWithMedicalRecordsAsync(int id);
        Task<Pet> GetWithVaccinationsAsync(int id);
        Task<Pet> GetWithAllDataAsync(int id);
        Task<IEnumerable<Pet>> SearchAsync(string searchTerm, int? clientId = null);
        Task<IEnumerable<Pet>> GetPetsWithUpcomingVaccinationsAsync(int daysAhead);
    }
}
