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

namespace VetClinicCRM.Data.Repositories
{
    public class PetRepository : Repository<Pet>, IPetRepository
    {
        public PetRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Pet>> GetByClientIdAsync(int clientId)
        {
            return await _dbSet
                .Where(p => p.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetBySpeciesAsync(PetSpecies species)
        {
            return await _dbSet
                .Where(p => p.Species == species)
                .ToListAsync();
        }

        public async Task<Pet> GetWithMedicalRecordsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pet> GetWithVaccinationsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Vaccinations)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pet> GetWithAllDataAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Client)
                .Include(p => p.MedicalRecords)
                .Include(p => p.Vaccinations)
                .ThenInclude(v => v.Reminders)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pet>> SearchAsync(string searchTerm, int? clientId = null)
        {
            var query = _dbSet.AsQueryable();

            if (clientId.HasValue)
            {
                query = query.Where(p => p.ClientId == clientId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) ||
                                       p.Breed.ToLower().Contains(searchTerm) ||
                                       p.Color.ToLower().Contains(searchTerm) ||
                                       (p.MicrochipNumber != null && p.MicrochipNumber.Contains(searchTerm)));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsWithUpcomingVaccinationsAsync(int daysAhead)
        {
            var targetDate = DateTime.Now.AddDays(daysAhead);

            return await _dbSet
                .Include(p => p.Vaccinations)
                .Where(p => p.Vaccinations.Any(v =>
                    v.NextVaccinationDate.HasValue &&
                    v.NextVaccinationDate.Value.Date <= targetDate.Date))
                .ToListAsync();
        }
    }
}
