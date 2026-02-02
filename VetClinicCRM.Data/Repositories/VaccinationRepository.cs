using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Interfaces;
using VetClinicCRM.Core.Models;
using VetClinicCRM.Data.Data;

namespace VetClinicCRM.Data.Repositories
{
    public class VaccinationRepository : Repository<Vaccination>, IVaccinationRepository
    {
        public VaccinationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vaccination>> GetByPetIdAsync(int petId)
        {
            return await _dbSet
                .Where(v => v.PetId == petId)
                .OrderByDescending(v => v.DateAdministered)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vaccination>> GetUpcomingVaccinationsAsync(int daysAhead)
        {
            var today = DateTime.Today;
            var targetDate = today.AddDays(daysAhead);

            return await _dbSet
                .Include(v => v.Pet)
                .ThenInclude(p => p.Client)
                .Where(v => v.NextVaccinationDate.HasValue &&
                           v.NextVaccinationDate.Value.Date >= today &&
                           v.NextVaccinationDate.Value.Date <= targetDate)
                .OrderBy(v => v.NextVaccinationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vaccination>> GetOverdueVaccinationsAsync()
        {
            var today = DateTime.Today;

            return await _dbSet
                .Include(v => v.Pet)
                .ThenInclude(p => p.Client)
                .Where(v => v.NextVaccinationDate.HasValue &&
                           v.NextVaccinationDate.Value.Date < today)
                .OrderBy(v => v.NextVaccinationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vaccination>> GetVaccinationsDueAsync(DateTime date)
        {
            return await _dbSet
                .Include(v => v.Pet)
                .ThenInclude(p => p.Client)
                .Where(v => v.NextVaccinationDate.HasValue &&
                           v.NextVaccinationDate.Value.Date == date.Date)
                .ToListAsync();
        }

        public async Task<Vaccination> GetWithPetAsync(int id)
        {
            return await _dbSet
                .Include(v => v.Pet)
                .ThenInclude(p => p.Client)
                .Include(v => v.Reminders)
                .Include(v => v.AdministeredBy)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vaccination>> GetByVaccineNameAsync(string vaccineName)
        {
            return await _dbSet
                .Where(v => v.VaccineName == vaccineName)
                .ToListAsync();
        }

        public async Task<bool> HasVaccinationAsync(int petId, string vaccineName, DateTime date)
        {
            return await _dbSet
                .AnyAsync(v => v.PetId == petId &&
                              v.VaccineName == vaccineName &&
                              v.DateAdministered.Date == date.Date);
        }
    }
}
