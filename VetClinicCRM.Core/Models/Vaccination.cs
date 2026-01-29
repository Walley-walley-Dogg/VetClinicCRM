using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetClinicCRM.Core.Models
{
    public class Vaccination
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string VaccineName { get; set; }

        [MaxLength(100)]
        public string? VaccineManufacturer { get; set; }

        [MaxLength(50)]
        public string? BatchNumber { get; set; }

        [Required]
        public DateTime DateAdministered { get; set; } = DateTime.Now;

        public DateTime? NextVaccinationDate { get; set; } // Рассчитывается автоматически

        public bool IsCompleted { get; set; } = true;

        [MaxLength(500)]
        public string? Notes { get; set; }

      
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public int? AdministeredById { get; set; } 
        public virtual User? AdministeredBy { get; set; }

        
        public virtual ICollection<VaccineReminder> Reminders { get; set; } = new List<VaccineReminder>();
    }
}
