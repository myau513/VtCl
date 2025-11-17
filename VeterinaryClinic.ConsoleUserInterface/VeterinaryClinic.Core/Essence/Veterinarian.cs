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

        /// <summary>
        /// Создает новый экземпляр ветеринара.
        /// </summary>
        /// <param name="id">Уникальный идентификатор ветеринара</param>
        /// <param name="fullName">Полное имя ветеринара</param>
        /// <param name="workDaysString">Строковое представление рабочих дней через запятую</param>
        public Veterinarian(Guid id, string fullName, string workDaysString)
        {
            Id = id;
            FullName = fullName;
            WorkDaysString = workDaysString;
        }

        [NotMapped]
        public DayOfWeek[] WorkDays => GetWorkDays();

        /// <summary>
        /// Преобразует строку рабочих дней в массив перечисления DayOfWeek.
        /// </summary>
        /// <returns>Массив рабочих дней ветеринара</returns>
        public DayOfWeek[] GetWorkDays()
        {
            return string.IsNullOrEmpty(WorkDaysString)
                ? new DayOfWeek[0]
                : WorkDaysString.Split(',').Select(d => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), d)).ToArray();
        }

        /// <summary>
        /// Устанавливает рабочие дни ветеринара из массива DayOfWeek.
        /// </summary>
        /// <param name="workDays">Массив рабочих дней для установки</param>
        public void SetWorkDays(DayOfWeek[] workDays)
        {
            WorkDaysString = workDays != null ? string.Join(",", workDays) : "";
        }

        /// <summary>
        /// Возвращает строковое представление ветеринара (фио).
        /// </summary>
        /// <returns>Полное имя ветеринара</returns>
        public override string ToString()
        {
            return FullName;
        }
    }
}