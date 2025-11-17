using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class VeterinarianManager
    {
        private List<VeterinarianDto> _veterinarians = new List<VeterinarianDto>();

        public VeterinarianManager()
        {
            InitializeVeterinarians();
        }

        private void InitializeVeterinarians()
        {
            CreateVeterinarian("Иванов И.И.", new[] { DayOfWeek.Monday, DayOfWeek.Thursday });
            CreateVeterinarian("Петров П.П.", new[] { DayOfWeek.Tuesday, DayOfWeek.Friday });
            CreateVeterinarian("Сидоров С.С.", new[] { DayOfWeek.Wednesday, DayOfWeek.Saturday });
        }

        public VeterinarianDto CreateVeterinarian(string fullName, DayOfWeek[] workDays)
        {
            var vet = new VeterinarianDto
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                WorkDaysString = workDays != null ? string.Join(",", workDays) : ""
            };

            _veterinarians.Add(vet);
            return vet;
        }

        public List<VeterinarianDto> GetAllVeterinarians() => _veterinarians;

        public VeterinarianDto GetVeterinarianById(Guid id) =>
            _veterinarians.FirstOrDefault(v => v.Id == id);
    }
}