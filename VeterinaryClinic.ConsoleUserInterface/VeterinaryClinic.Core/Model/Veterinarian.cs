using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Models
{
    public class Veterinarian
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DayOfWeek[] WorkDays { get; set; }

        public Veterinarian(int id, string fullName, DayOfWeek[] workDays)
        {
            Id = id;
            FullName = fullName;
            WorkDays = workDays;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
