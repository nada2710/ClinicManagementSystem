using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection.Emit;
using System.Reflection;
using ClinicInfrastructure.Configuration;

namespace ClinicInfrastructure.DbHelper.Context
{
    public class ClinicDbContext(DbContextOptions<ClinicDbContext> options): IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PrescriptionMedicine>()
                   .HasKey(pm => new { pm.PrescriptionId, pm.MedicineId });

            builder.ApplySoftDeleteQueryFilter();
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            UserTablesConfiguration.ConfigureAll(builder);
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<AvailableTime> AvailableTime { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<MedicalRecord> MedicalRecord { get; set; }
        public DbSet<Medicine> Medicine { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }
    }
}
