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
        /// <summary>
        /// Набор данных назначений
        /// </summary>
        public DbSet<AppointmentDto> Appointments { set; get; }

        /// <summary>
        /// Набор данных владельцев
        /// </summary>
        public DbSet<OwnerDto> Owners { get; set; }

        /// <summary>
        /// Набор данных питомцев
        /// </summary>
        public DbSet<PetDto> Pets { get; set; }

        /// <summary>
        /// Набор данных ветеринаров
        /// </summary>
        public DbSet<VeterinarianDto> Veterinarians { get; set; }

        /// <summary>
        /// Набор данных истории посещений
        /// </summary>
        public DbSet<VisitHistoryDto> VisitHistories { get; set; }

        /// <summary>
        /// Инициализирует контекст с опциями
        /// </summary>
        /// <param name="options">Опции контекста базы данных</param>
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        /// <summary>
        /// Создает базу данных если она не существует
        /// </summary>
        public void CreateDatabase()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Настраивает модель базы данных
        /// </summary>
        /// <param name="modelBuilder">Построитель модели</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OwnerDto>(entity =>
            {
                entity.HasKey(owner => owner.Id);
                entity.Property(owner => owner.Id)
                    .HasColumnType("uniqueidentifier") 
                    .HasDefaultValueSql("NEWID()") 
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
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();
                entity.Property(pet => pet.OwnerId)
                    .HasColumnType("uniqueidentifier"); 
                entity.Property(pet => pet.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(pet => pet.Species)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(pet => pet.Breed)
                    .HasMaxLength(100);

                entity.HasOne<OwnerDto>()
                    .WithMany()
                    .HasForeignKey(pet => pet.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<VeterinarianDto>(entity =>
            {
                entity.HasKey(veterinarian => veterinarian.Id);
                entity.Property(veterinarian => veterinarian.Id)
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("NEWID()")
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
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();
                entity.Property(appointment => appointment.PetId)
                    .HasColumnType("uniqueidentifier");
                entity.Property(appointment => appointment.VeterinarianId)
                    .HasColumnType("uniqueidentifier");
                entity.Property(appointment => appointment.AppointmentDate)
                    .IsRequired();
                entity.Property(appointment => appointment.TimeSlot)
                    .HasMaxLength(50);
                entity.Property(appointment => appointment.Reason)
                    .HasMaxLength(500);
                entity.Property(appointment => appointment.Breed)
                    .HasMaxLength(100);

                entity.HasOne<PetDto>()
                    .WithMany()
                    .HasForeignKey(appointment => appointment.PetId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<VeterinarianDto>()
                    .WithMany()
                    .HasForeignKey(appointment => appointment.VeterinarianId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<VisitHistoryDto>(entity =>
            {
                entity.ToTable("VisitHistories"); 

                entity.HasKey(visitHistory => visitHistory.Id);
                entity.Property(visitHistory => visitHistory.Id)
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();
                entity.Property(visitHistory => visitHistory.PetId)
                    .HasColumnType("uniqueidentifier");
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

                entity.HasOne<PetDto>()
                    .WithMany()
                    .HasForeignKey(visitHistory => visitHistory.PetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        /// <summary>
        /// Удаляет базу данных если она существует
        /// </summary>
        public void DeleteDatabase()
        {
            Database.EnsureDeleted();
        }
    }
}