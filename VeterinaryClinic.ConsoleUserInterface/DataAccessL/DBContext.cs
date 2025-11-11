using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public class VetClinicDbContext : DbContext
    {
        public VetClinicDbContext() : base("VetClinicDbContext")
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Veterinarian> Veterinarians { get; set; }
        public DbSet<VisitHistory> VisitHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Конфигурация для Appointment
            modelBuilder.Entity<Appointment>()
                .HasKey(appointment => appointment.Id_db);

            modelBuilder.Entity<Appointment>()
                .Property(appointment => appointment.AppointmentDate)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(appointment => appointment.TimeSlot)
                .HasMaxLength(50);

            modelBuilder.Entity<Appointment>()
                .Property(appointment => appointment.Reason)
                .HasMaxLength(500);

            modelBuilder.Entity<Appointment>()
                .Property(appointment => appointment.Breed)
                .HasMaxLength(100);

            // Конфигурация для Owner
            modelBuilder.Entity<Owner>()
                .HasKey(owner => owner.Id_db);

            modelBuilder.Entity<Owner>()
                .Property(owner => owner.FullName)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Owner>()
                .Property(owner => owner.PhoneNumber)
                .HasMaxLength(20);

            // Конфигурация для Pet
            modelBuilder.Entity<Pet>()
                .HasKey(pet => pet.Id_db);

            modelBuilder.Entity<Pet>()
                .Property(pet => pet.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Pet>()
                .Property(pet => pet.Species)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Pet>()
                .Property(pet => pet.Breed)
                .HasMaxLength(100);

            // Конфигурация для Veterinarian
            modelBuilder.Entity<Veterinarian>()
                .HasKey(veterinarian => veterinarian.Id_db);

            modelBuilder.Entity<Veterinarian>()
                .Property(veterinarian => veterinarian.FullName)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Veterinarian>()
                .Property(veterinarian => veterinarian.WorkDaysString)
                .HasMaxLength(100); // Будем хранить как строку "Monday,Tuesday,Friday"

            // Конфигурация для VisitHistory
            modelBuilder.Entity<VisitHistory>()
                .HasKey(visitHistory => visitHistory.Id_db);

            modelBuilder.Entity<VisitHistory>()
                .Property(visitHistory => visitHistory.VeterinarianName)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<VisitHistory>()
                .Property(visitHistory => visitHistory.VisitDate)
                .IsRequired();

            modelBuilder.Entity<VisitHistory>()
                .Property(visitHistory => visitHistory.Reason)
                .HasMaxLength(500);

            modelBuilder.Entity<VisitHistory>()
                .Property(visitHistory => visitHistory.Diagnosis)
                .HasMaxLength(1000);

            modelBuilder.Entity<VisitHistory>()
                .Property(visitHistory => visitHistory.Treatment)
                .HasMaxLength(1000);

            modelBuilder.Entity<VisitHistory>()
                .Property(visitHistory => visitHistory.Notes)
                .HasMaxLength(2000);

            base.OnModelCreating(modelBuilder);
        }
    }
}