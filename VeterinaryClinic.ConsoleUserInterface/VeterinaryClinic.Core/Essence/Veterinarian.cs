using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Essence
{
    public class Veterinarian : IDomainObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string WorkDaysString { get; set; }
        public Veterinarian(Guid id, string fullName, string workDaysString)
        {
            Id = id;
            FullName = fullName;
            WorkDaysString = workDaysString;
        }

        [NotMapped]
        public DayOfWeek[] WorkDays => GetWorkDays();

        public DayOfWeek[] GetWorkDays()
        {
            return string.IsNullOrEmpty(WorkDaysString)
                ? new DayOfWeek[0]
                : WorkDaysString.Split(',').Select(d => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), d)).ToArray();
        }

        public void SetWorkDays(DayOfWeek[] workDays)
        {
            WorkDaysString = workDays != null ? string.Join(",", workDays) : "";
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
