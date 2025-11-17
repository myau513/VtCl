using System;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.DTO
{
    public class VeterinarianDto : Veterinarian
    {
        public Guid Id { get; set; }
        public int VeterinarianId { get; set; }
        public string FullName { get; set; }

        // Для работы с днями в логике
        public DayOfWeek[] WorkDays { get; set; }

        // Для сохранения в базу данных
        public string WorkDaysString
        {
            get => WorkDays != null ? string.Join(",", WorkDays.Select(d => d.ToString())) : "";
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    WorkDays = new DayOfWeek[0];
                }
                else
                {
                    WorkDays = value.Split(',')
                        .Select(dayStr => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayStr.Trim()))
                        .ToArray();
                }
            }
        }

        public VeterinarianDto()
        {
            WorkDays = new DayOfWeek[0];
        }
    }
}