using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class PetManager
    {
        private List<Pet> petsList = new List<Pet>();
        private int nextPetId = 1;
        private readonly OwnerManager ownerManager;

        /// <summary>
        /// Создает менеджер питомцев с ссылкой на менеджер владельцев
        /// </summary>
        /// <param name="ownerManager">Менеджер для проверки существования владельцев</param>
        public PetManager(OwnerManager ownerManager)
        {
            this.ownerManager = ownerManager;
        }

        /// <summary>
        /// Создает нового питомца
        /// </summary>
        /// <param name="name">Кличка питомца</param>
        /// <param name="species">Вид животного</param>
        /// <param name="breed">Порода животного</param>
        /// <param name="ownerId">ID владельца</param>
        /// <returns>Созданный питомец</returns>
        public Pet CreatePet(string name, string species, string breed, int ownerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Кличка не может быть пустая");

            if (string.IsNullOrWhiteSpace(species))
                throw new ArgumentException("Вид не может быть пустым");

            if (string.IsNullOrWhiteSpace(breed))
                throw new ArgumentException("Порода не может быть пустая");

            if (!ownerManager.OwnerExists(ownerId))
                throw new ArgumentException($"Владелец с ID {ownerId} не найден");


            Pet newPet = new Pet(nextPetId++, name.Trim(), species.Trim(), breed.Trim(), ownerId);

            petsList.Add(newPet);
            return newPet;
        }

        /// <summary>
        /// Находит питомца по ID
        /// </summary>
        /// <param name="id">ID питомца</param>
        /// <returns>Найденный питомец или null</returns>
        public Pet GetPet(int id)
        {
            return petsList.FirstOrDefault(pet => pet.Id == id);
        }

        /// <summary>
        /// Получает список всех питомцев
        /// </summary>
        /// <returns>Список всех питомцев</returns>
        public List<Pet> GetAllPets()
        {
            return petsList;
        }

        /// <summary>
        /// Обновляет информацию о питомце
        /// </summary>
        /// <param name="pet">Объект питомца с обновленными данными</param>
        public void UpdatePet(Pet pet)
        {
            Pet existingPet = GetPet(pet.Id);
            if (existingPet == null)
                throw new ArgumentException($"Питомец с ID {pet.Id} не найден");

            if (string.IsNullOrWhiteSpace(pet.Name))
                throw new ArgumentException("Имя питомца не может быть пустым");

            if (string.IsNullOrWhiteSpace(pet.Species))
                throw new ArgumentException("Вид не может быть пустым");

            if (string.IsNullOrWhiteSpace(pet.Breed))
                throw new ArgumentException("Порода не может быть пустая");

            if (!ownerManager.OwnerExists(pet.OwnerId))
                throw new ArgumentException($"Владелец с ID {pet.OwnerId} не найден");

            existingPet.Name = pet.Name.Trim();
            existingPet.Species = pet.Species.Trim();
            existingPet.Breed = pet.Breed.Trim();
            existingPet.OwnerId = pet.OwnerId;
        }

        /// <summary>
        /// Удаляет питомца по ID
        /// </summary>
        /// <param name="id">ID питомца для удаления</param>
        /// <returns>True если удаление успешно, иначе False</returns>
        public bool DeletePet(int id)
        {
            Pet petToRemove = GetPet(id);
            if (petToRemove != null)
            {
                petsList.Remove(petToRemove);
                Console.WriteLine($"Питомец с ID {id} успешно удален");
                return true;
            }
            else
            {
                Console.WriteLine($"Питомец с ID {id} не найден");
                return false;
            }
        }

        /// <summary>
        /// Получает всех питомцев указанного владельца
        /// </summary>
        /// <param name="ownerId">ID владельца</param>
        /// <returns>Список питомцев владельца</returns>
        public List<Pet> GetPetsByOwnerId(int ownerId)
        {
            if (!ownerManager.OwnerExists(ownerId))
                throw new ArgumentException($"Владелец с ID {ownerId} не найден");

            return petsList.Where(pet => pet.OwnerId == ownerId).ToList();
        }

        /// <summary>
        /// Проверяет существование питомца по ID
        /// </summary>
        /// <param name="id">ID питомца для проверки</param>
        /// <returns>True если питомец существует, иначе False</returns>
        public bool PetExists(int id)
        {
            return petsList.Any(pet => pet.Id == id);
        }
    }
}