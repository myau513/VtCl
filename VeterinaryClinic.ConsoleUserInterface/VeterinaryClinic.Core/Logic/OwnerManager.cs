using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class OwnerManager
    {
        private List<Owner> ownersList = new List<Owner>();
        private int nextOwnerId = 1;

        /// <summary>
        /// Создает нового владельца животного
        /// </summary>
        /// <param name="fullName">Полное имя владельца</param>
        /// <param name="phoneNumber">Номер телефона владельца</param>
        /// <returns>Созданный владелец</returns>
        public Owner CreateOwner(string fullName, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("ФИО не может быть пустым");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Номер телефона не может быть пустым");

            Owner newOwner = new Owner(nextOwnerId++, fullName.Trim(), phoneNumber.Trim());

            ownersList.Add(newOwner);
            return newOwner;
        }

        /// <summary>
        /// Находит владельца по ID
        /// </summary>
        /// <param name="id">ID владельца</param>
        /// <returns>Найденный владелец или null</returns>
        public Owner GetOwner(int id)
        {
            return ownersList.FirstOrDefault(owner => owner.Id == id);
        }

        /// <summary>
        /// Получает список всех владельцев
        /// </summary>
        /// <returns>Список всех владельцев</returns>
        public List<Owner> GetAllOwners()
        {
            return ownersList;
        }

        /// <summary>
        /// Обновляет информацию о владельце
        /// </summary>
        /// <param name="owner">Объект владельца с обновленными данными</param>
        public void UpdateOwner(Owner owner)
        {
            Owner existingOwner = GetOwner(owner.Id);
            if (existingOwner == null)
                throw new ArgumentException($"Владелец с ID {owner.Id} не найден");

            if (string.IsNullOrWhiteSpace(owner.FullName))
                throw new ArgumentException("ФИО не может быть пустым");

            if (string.IsNullOrWhiteSpace(owner.PhoneNumber))
                throw new ArgumentException("Номер телефона не может быть пустым");

            existingOwner.FullName = owner.FullName.Trim();
            existingOwner.PhoneNumber = owner.PhoneNumber.Trim();
        }

        /// <summary>
        /// Удаляет владельца по ID
        /// </summary>
        /// <param name="id">ID владельца для удаления</param>
        /// <returns>True если удаление успешно, иначе False</returns>
        public bool DeleteOwner(int id)
        {
            Owner ownerToRemove = GetOwner(id);
            if (ownerToRemove != null)
            {
                ownersList.Remove(ownerToRemove);
                Console.WriteLine($"Владелец с ID {id} успешно удален");
                return true;
            }
            else
            {
                Console.WriteLine($"Владелец с ID {id} не найден");
                return false;
            }
        }

        /// <summary>
        /// Ищет владельцев по имени (частичное совпадение без учета регистра)
        /// </summary>
        /// <param name="name">Имя или часть имени для поиска</param>
        /// <returns>Список найденных владельцев</returns>
        public List<Owner> FindOwnersByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Owner>();

            return ownersList
                .Where(owner => owner.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        /// <summary>
        /// Проверяет существование владельца по ID
        /// </summary>
        /// <param name="id">ID владельца для проверки</param>
        /// <returns>True если владелец существует, иначе False</returns>
        public bool OwnerExists(int id)
        {
            return ownersList.Any(owner => owner.Id == id);
        }
    }
}