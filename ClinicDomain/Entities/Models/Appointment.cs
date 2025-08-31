using ClinicDomain.Entities.Base;
using ClinicDomain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointment:BaseEntity<int>
    {
        public DateTime AppointmentDate { get; set; }
        
        [MaxLength(1000)]
        public string Notes { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public Invoice? Invoice { get; set; }
        public Prescription? Prescription { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        
        [ForeignKey("AvailableTime")]
        public int AvailableTimeId { get; set; }
        public AvailableTime AvailableTime { get; set; }
    }
  
}
