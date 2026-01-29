using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VetClinicCRM.Core.Models;

namespace VetClinicCRM.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
      
        public DbSet<Client> Clients { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<VaccineReminder> VaccineReminders { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Только для миграций
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=VetClinicCRM;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(c => c.PhoneNumber).IsUnique();
                entity.Property(c => c.LastName).IsRequired().HasMaxLength(50);
                entity.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(c => c.Email).HasMaxLength(100);
            });

       
            modelBuilder.Entity<Pet>(entity =>
            {
                entity.HasOne(p => p.Client)
                      .WithMany(c => c.Pets)
                      .HasForeignKey(p => p.ClientId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });

   
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasOne(mr => mr.Pet)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(mr => mr.PetId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

         
            modelBuilder.Entity<Vaccination>(entity =>
            {
                entity.HasOne(v => v.Pet)
                      .WithMany(p => p.Vaccinations)
                      .HasForeignKey(v => v.PetId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

     
            modelBuilder.Entity<VaccineReminder>(entity =>
            {
                entity.HasOne(vr => vr.Vaccination)
                      .WithMany(v => v.Reminders)
                      .HasForeignKey(vr => vr.VaccinationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

           
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            });

           
            //SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123", salt),
                    Salt = salt,
                    FullName = "Администратор",
                    Role = Core.Enums.UserRole.Administrator,
                    Email = "admin@vetclinic.local",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}
