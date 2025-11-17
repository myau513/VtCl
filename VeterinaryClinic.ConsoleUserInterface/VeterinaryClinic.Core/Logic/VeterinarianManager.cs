using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class VeterinarianManager
    {
        private List<VeterinarianDto> _veterinarians = new List<VeterinarianDto>();

        /// <summary>
        /// Инициализирует менеджер ветеринаров и создает базовых ветеринаров
        /// </summary>
        public VeterinarianManager()
        {
            InitializeVeterinarians();
        }

        /// <summary>
        /// Инициализирует базовых ветеринаров в системе
        /// </summary>
        private void InitializeVeterinarians()
        {
            CreateVeterinarian("Иванов И.И.", new[] { DayOfWeek.Monday, DayOfWeek.Thursday });
            CreateVeterinarian("Петров П.П.", new[] { DayOfWeek.Tuesday, DayOfWeek.Friday });
            CreateVeterinarian("Сидоров С.С.", new[] { DayOfWeek.Wednesday, DayOfWeek.Saturday });
        }

        /// <summary>
        /// Создает нового ветеринара
        /// </summary>
        /// <param name="fullName">ФИО ветеринара</param>
        /// <param name="workDays">Рабочие дни ветеринара</param>
        /// <returns>Созданный объект ветеринара</returns>
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

        /// <summary>
        /// Возвращает всех ветеринаров
        /// </summary>
        /// <returns>Список всех ветеринаров</returns>
        public List<VeterinarianDto> GetAllVeterinarians() => _veterinarians;

        /// <summary>
        /// Находит ветеринара по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор ветеринара</param>
        /// <returns>Найденный ветеринар или null</returns>
        public VeterinarianDto GetVeterinarianById(Guid id) =>
            _veterinarians.FirstOrDefault(v => v.Id == id);
    }
}