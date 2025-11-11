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
        public Guid Id_db { get; set; } = Guid.NewGuid();
        public int Id { get; set; }
        public string FullName { get; set; }
        public string WorkDaysString { get; set; }

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

        public Veterinarian(int id, string fullName, DayOfWeek[] workDays)
        {
            Id = id;
            FullName = fullName;
            SetWorkDays(workDays);
        }
        public Veterinarian()
        {
        }

        /// <summary>
        /// Возвращает строковое представление ветеринара - его полное имя
        /// </summary>
        /// <returns>Полное имя ветеринара</returns>

        public override string ToString()
        {
            return FullName;
        }

    }
}
