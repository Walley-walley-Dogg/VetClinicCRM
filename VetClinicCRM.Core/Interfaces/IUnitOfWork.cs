using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicCRM.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Репозитории
        IClientRepository Clients { get; }
        IPetRepository Pets { get; }
        IMedicalRecordRepository MedicalRecords { get; }
        IVaccinationRepository Vaccinations { get; }
        IVaccineReminderRepository VaccineReminders { get; }
        IUserRepository Users { get; }

        // Сохранение изменений
        Task<int> CompleteAsync();

        // Начало транзакции
        Task BeginTransactionAsync();

        // Фиксация транзакции
        Task CommitTransactionAsync();

        // Откат транзакции
        Task RollbackTransactionAsync();
    }
}
