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
    public class VaccineReminderRepository : Repository<VaccineReminder>, IVaccineReminderRepository
    {
        public VaccineReminderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VaccineReminder>> GetPendingRemindersAsync()
        {
            return await _dbSet
                .Include(vr => vr.Vaccination)
                .ThenInclude(v => v.Pet)
                .ThenInclude(p => p.Client)
                .Where(vr => !vr.IsSent && vr.ReminderDate <= DateTime.Now)
                .OrderBy(vr => vr.ReminderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccineReminder>> GetRemindersByVaccinationIdAsync(int vaccinationId)
        {
            return await _dbSet
                .Where(vr => vr.VaccinationId == vaccinationId)
                .OrderBy(vr => vr.ReminderDate)
            .ToListAsync();
        }

        public async Task<IEnumerable<VaccineReminder>> GetDueRemindersAsync(DateTime date)
        {
            return await _dbSet
                .Include(vr => vr.Vaccination)
                .ThenInclude(v => v.Pet)
                .ThenInclude(p => p.Client)
                .Where(vr => !vr.IsSent && vr.ReminderDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccineReminder>> GetSentRemindersAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(vr => vr.Vaccination)
                .Where(vr => vr.IsSent &&
                           vr.SentDate >= startDate &&
                           vr.SentDate <= endDate)
                .ToListAsync();
        }

        public async Task MarkAsSentAsync(int reminderId)
        {
            var reminder = await GetByIdAsync(reminderId);
            if (reminder != null)
            {
                reminder.IsSent = true;
                reminder.SentDate = DateTime.Now;
                await UpdateAsync(reminder);
            }
        }

        public async Task<int> GetPendingCountAsync()
        {
            return await _dbSet
                .CountAsync(vr => !vr.IsSent && vr.ReminderDate <= DateTime.Now);
        }
    }
}
