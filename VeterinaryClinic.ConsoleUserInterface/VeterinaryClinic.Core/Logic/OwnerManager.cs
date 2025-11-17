using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class OwnerManager
    {
        private List<OwnerDto> _owners = new List<OwnerDto>();

        /// <summary>
        /// Создает нового владельца
        /// </summary>
        /// <param name="fullName">ФИО владельца</param>
        /// <param name="phoneNumber">Номер телефона владельца</param>
        /// <returns>Созданный объект владельца</returns>
        public OwnerDto CreateOwner(string fullName, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("ФИО не может быть пустым");
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Номер телефона не может быть пустым");

            var owner = new OwnerDto
            {
                Id = Guid.NewGuid(),
                FullName = fullName.Trim(),
                PhoneNumber = phoneNumber.Trim()
            };

            _owners.Add(owner);
            return owner;
        }

        /// <summary>
        /// Возвращает всех владельцев
        /// </summary>
        /// <returns>Список всех владельцев</returns>
        public List<OwnerDto> GetAllOwners() => _owners;

        /// <summary>
        /// Находит владельца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор владельца</param>
        /// <returns>Найденный владелец или null</returns>
        public OwnerDto GetOwnerById(Guid id) => _owners.FirstOrDefault(o => o.Id == id);

        /// <summary>
        /// Обновляет данные владельца
        /// </summary>
        /// <param name="ownerDto">Объект с обновленными данными владельца</param>
        public void UpdateOwner(OwnerDto ownerDto)
        {
            var existing = GetOwnerById(ownerDto.Id);
            if (existing == null)
                throw new ArgumentException("Владелец не найден");

            existing.FullName = ownerDto.FullName.Trim();
            existing.PhoneNumber = ownerDto.PhoneNumber.Trim();
        }

        /// <summary>
        /// Удаляет владельца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемого владельца</param>
        public void DeleteOwner(Guid id)
        {
            var owner = GetOwnerById(id);
            if (owner != null)
                _owners.Remove(owner);
            else
                throw new ArgumentException("Владелец не найден");
        }

        /// <summary>
        /// Проверяет существование владельца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор владельца</param>
        /// <returns>True если владелец существует, иначе False</returns>
        public bool OwnerExists(Guid id) => _owners.Any(o => o.Id == id);
    }
}