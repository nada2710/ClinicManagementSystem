
using ClinicDomain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string Gender { get; set; }
        public bool IsDeleted { get; set; }
        public TypeUser UserType { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public string VerificationCode { get; set; }
        public DateTime? VerificationCodeExpiration { get; set; }
    }
    public class Doctor :ApplicationUser
    {
        public string? Specialization { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
        public ICollection<Appointment> appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<AvailableTime> availableTimes { get; set; } = new HashSet<AvailableTime>();
        public ICollection<Rating> ratings { get; set; } = new HashSet<Rating>();
        public ICollection<MedicalRecord> medicalRecords { get; set; } = new HashSet<MedicalRecord>();
    }
    public class Patient : ApplicationUser
    {
        public DateTime? DateOfBirth { get; set; }
        public string? EmergencyContact { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
        
        public ICollection<Appointment> appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<Invoice> invoices { get; set; } = new HashSet<Invoice>();
        public ICollection<MedicalRecord> medicalRecords { get; set; } = new HashSet<MedicalRecord>();
        public ICollection<Rating> ratings { get; set; } = new HashSet<Rating>();
    }
    public class Admin : ApplicationUser { }
  
}
