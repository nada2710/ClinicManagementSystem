using ClinicDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating:BaseEntity<int>
    {
        [Range(1, 5, ErrorMessage = "Value must be between 1 and 5.")]
        public int Value { get; set; } 
        public string Comment { get; set; }
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
