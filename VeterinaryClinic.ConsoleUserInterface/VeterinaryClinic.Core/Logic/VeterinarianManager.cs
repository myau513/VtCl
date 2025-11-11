using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class VeterinarianManager
    {
        private List<Veterinarian> veterinarians = new List<Veterinarian>();
        private int nextVetId = 1;

        public VeterinarianManager()
        {
            InitializeVeterinarians();
        }

        private void InitializeVeterinarians()
        {
            veterinarians.Add(new Veterinarian(nextVetId++, "Иванов И.И.", new[] { DayOfWeek.Monday, DayOfWeek.Thursday }));
            veterinarians.Add(new Veterinarian(nextVetId++, "Петров П.П.", new[] { DayOfWeek.Tuesday, DayOfWeek.Friday }));
            veterinarians.Add(new Veterinarian(nextVetId++, "Сидоров С.С.", new[] { DayOfWeek.Wednesday, DayOfWeek.Saturday }));
        }

        public List<Veterinarian> GetAllVeterinarians() => veterinarians;
        public Veterinarian GetVeterinarian(int id) => veterinarians.FirstOrDefault(v => v.Id == id);
        public Veterinarian GetVeterinarianByDay(DayOfWeek day) => veterinarians.FirstOrDefault(v => v.WorkDays.Contains(day));
    }
}