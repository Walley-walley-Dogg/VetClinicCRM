using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Interfaces;
using VetClinicCRM.Data.Interfaces;
using VetClinicCRM.Data.Repositories;

namespace VetClinicCRM.Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        // Репозитории
        private IClientRepository _clientRepository;
        private IPetRepository _petRepository;
        private IMedicalRecordRepository _medicalRecordRepository;
        private IVaccinationRepository _vaccinationRepository;
        private IVaccineReminderRepository _vaccineReminderRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IClientRepository Clients =>
            _clientRepository ??= new ClientRepository(_context);

        public IPetRepository Pets =>
            _petRepository ??= new PetRepository(_context);

        public IMedicalRecordRepository MedicalRecords =>
            _medicalRecordRepository ??= new MedicalRecordRepository(_context);

        public IVaccinationRepository Vaccinations =>
            _vaccinationRepository ??= new VaccinationRepository(_context);

        public IVaccineReminderRepository VaccineReminders =>
            _vaccineReminderRepository ??= new VaccineReminderRepository(_context);

        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
