using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Enums;

namespace VetClinicCRM.Core.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public PetSpecies Species { get; set; }

        [MaxLength(100)]
        public string? Breed { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderType Gender { get; set; } = GenderType.Unknown;

        [MaxLength(50)]
        public string? Color { get; set; }

        [MaxLength(50)]
        public string? MicrochipNumber { get; set; }

        [MaxLength(500)]
        public string? SpecialNotes { get; set; } // Аллергии, хронические болезни и т.д.

       
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
        public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();

     
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue)
                    return null;

                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
