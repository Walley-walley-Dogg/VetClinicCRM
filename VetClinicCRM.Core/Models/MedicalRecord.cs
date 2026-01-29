using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Enums;

namespace VetClinicCRM.Core.Models
{
    public class MedicalRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime VisitDate { get; set; } = DateTime.Now;

        public VisitType VisitType { get; set; }

        [Required]
        [MaxLength(500)]
        public string Diagnosis { get; set; }

        [MaxLength(1000)]
        public string? Symptoms { get; set; }

        [MaxLength(1000)]
        public string? Treatment { get; set; }

        [MaxLength(500)]
        public string? PrescribedMedications { get; set; }

        [MaxLength(1000)]
        public string? DoctorNotes { get; set; }

        public decimal? Weight { get; set; } 
        public decimal? Temperature { get; set; } 

        
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public int? VeterinarianId { get; set; } 
        public virtual User? Veterinarian { get; set; }
    }
}
