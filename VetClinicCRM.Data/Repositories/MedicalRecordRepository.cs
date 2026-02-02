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
    public class MedicalRecordRepository : Repository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MedicalRecord>> GetByPetIdAsync(int petId)
        {
            return await _dbSet
                .Where(mr => mr.PetId == petId)
                .OrderByDescending(mr => mr.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetByVeterinarianIdAsync(int veterinarianId)
        {
            return await _dbSet
                .Where(mr => mr.VeterinarianId == veterinarianId)
                .OrderByDescending(mr => mr.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(mr => mr.VisitDate >= startDate && mr.VisitDate <= endDate)
                .OrderByDescending(mr => mr.VisitDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetRecentRecordsAsync(int count)
        {
            return await _dbSet
                .Include(mr => mr.Pet)
                .ThenInclude(p => p.Client)
                .OrderByDescending(mr => mr.VisitDate)
            .Take(count)
                .ToListAsync();
        }

        public async Task<MedicalRecord> GetWithPetAsync(int id)
        {
            return await _dbSet
                .Include(mr => mr.Pet)
                .ThenInclude(p => p.Client)
                .Include(mr => mr.Veterinarian)
                .FirstOrDefaultAsync(mr => mr.Id == id);
        }
    }
}
