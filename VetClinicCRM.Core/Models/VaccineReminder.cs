using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetClinicCRM.Core.Enums;

namespace VetClinicCRM.Core.Models
{
    public class VaccineReminder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ReminderDate { get; set; }

        public ReminderType ReminderType { get; set; }

        public bool IsSent { get; set; } = false;
        public DateTime? SentDate { get; set; }

        [MaxLength(500)]
        public string? Message { get; set; }

     
        public int VaccinationId { get; set; }
        public virtual Vaccination Vaccination { get; set; }
    }
}
