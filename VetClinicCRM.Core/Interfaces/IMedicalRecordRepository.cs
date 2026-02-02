using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Models;

namespace VetClinicCRM.Core.Interfaces
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecord>
    {
        // Специфичные методы для медицинских записей
        Task<IEnumerable<MedicalRecord>> GetByPetIdAsync(int petId);
        Task<IEnumerable<MedicalRecord>> GetByVeterinarianIdAsync(int veterinarianId);
        Task<IEnumerable<MedicalRecord>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MedicalRecord>> GetRecentRecordsAsync(int count);
        Task<MedicalRecord> GetWithPetAsync(int id);
    }
}
