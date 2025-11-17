using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;

namespace DataAccessL
{
    public class Context : DbContext
    {

        public DbSet<AppointmentDto> Appointments { set; get; }
        public DbSet<OwnerDto> Owners { get; set; }
        public DbSet<PetDto> Pets { get; set; }
        public DbSet<VeterinarianDto> Veterinarians { get; set; }
        public DbSet<VisitHistoryDto> VisitHistories { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public void CreateDatabase()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конвертация Guid для всех сущностей
            modelBuilder.Entity<OwnerDto>(entity =>
            {
                entity.HasKey(owner => owner.Id);
                entity.Property(owner => owner.Id)
                    .HasConversion(
                        v => v.ToByteArray(),  // Конвертируем Guid в byte[]
                        v => new Guid(v))      // Конвертируем byte[] в Guid
                    .ValueGeneratedOnAdd();
                entity.Property(owner => owner.FullName)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(owner => owner.PhoneNumber)
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<PetDto>(entity =>
            {
                entity.HasKey(pet => pet.Id);
                entity.Property(pet => pet.Id)
                    .HasConversion(
                        v => v.ToByteArray(),
                        v => new Guid(v))
                    .ValueGeneratedOnAdd();
                entity.Property(pet => pet.OwnerId)
                    .HasConversion(
                        v => v.ToByteArray(),
                        v => new Guid(v));
                entity.Property(pet => pet.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(pet => pet.Species)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(pet => pet.Breed)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<VeterinarianDto>(entity =>
            {
                entity.HasKey(veterinarian => veterinarian.Id);
                entity.Property(veterinarian => veterinarian.Id)
                    .HasConversion(
                        v => v.ToByteArray(),
                        v => new Guid(v))
                    .ValueGeneratedOnAdd();
                entity.Property(veterinarian => veterinarian.FullName)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(veterinarian => veterinarian.WorkDaysString)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AppointmentDto>(entity =>
            {
                entity.HasKey(appointment => appointment.Id);
                entity.Property(appointment => appointment.Id)
                    .HasConversion(
                        v => v.ToByteArray(),
                        v => new Guid(v))
                    .ValueGeneratedOnAdd();
                entity.Property(appointment => appointment.PetId)
                    .HasConversion(
                        v => v.ToByteArray(),
                        v => new Guid(v));
                entity.Property(appointment => appointment.VeterinarianId)
                    .HasConversion(
                        v => v.ToByteArray(),
                        v => new Guid(v));
                entity.Property(appointment => appointment.AppointmentDate)
                    .IsRequired();
                entity.Property(appointment => appointment.TimeSlot)
                    .HasMaxLength(50);
                entity.Property(appointment => appointment.Reason)
                    .HasMaxLength(500);
                entity.Property(appointment => appointment.Breed)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<VisitHistoryDto>(entity =>
            {
                entity.HasKey(visitHistory => visitHistory.Id);
                entity.Property(visitHistory => visitHistory.Id)
                    .HasConversion(
                        v => v.ToByteArray(),
                        v => new Guid(v))
                    .ValueGeneratedOnAdd();
                entity.Property(visitHistory => visitHistory.PetId)
                    .HasConversion(
                        v => v.ToByteArray(),
                        v => new Guid(v));
                entity.Property(visitHistory => visitHistory.VeterinarianName)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(visitHistory => visitHistory.VisitDate)
                    .IsRequired();
                entity.Property(visitHistory => visitHistory.Reason)
                    .HasMaxLength(500);
                entity.Property(visitHistory => visitHistory.Diagnosis)
                    .HasMaxLength(1000);
                entity.Property(visitHistory => visitHistory.Treatment)
                    .HasMaxLength(1000);
                entity.Property(visitHistory => visitHistory.Notes)
                    .HasMaxLength(2000);
            });
        }
    }
}