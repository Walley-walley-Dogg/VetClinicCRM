using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Models;

namespace VetClinicCRM.Core.Interfaces
{
    public interface IVaccineReminderRepository : IRepository<VaccineReminder>
    {
        // Специфичные методы для напоминаний
        Task<IEnumerable<VaccineReminder>> GetPendingRemindersAsync();
        Task<IEnumerable<VaccineReminder>> GetRemindersByVaccinationIdAsync(int vaccinationId);
        Task<IEnumerable<VaccineReminder>> GetDueRemindersAsync(DateTime date);
        Task<IEnumerable<VaccineReminder>> GetSentRemindersAsync(DateTime startDate, DateTime endDate);
        Task MarkAsSentAsync(int reminderId);
        Task<int> GetPendingCountAsync();
    }
}
