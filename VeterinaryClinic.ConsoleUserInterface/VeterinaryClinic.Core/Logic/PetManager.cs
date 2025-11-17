using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class PetManager
    {
        private List<PetDto> _pets = new List<PetDto>();
        private readonly OwnerManager _ownerManager;

        /// <summary>
        /// Инициализирует менеджер питомцев с менеджером владельцев
        /// </summary>
        /// <param name="ownerManager">Менеджер владельцев</param>
        public PetManager(OwnerManager ownerManager)
        {
            _ownerManager = ownerManager;
        }

        /// <summary>
        /// Создает нового питомца
        /// </summary>
        /// <param name="name">Кличка питомца</param>
        /// <param name="species">Вид питомца</param>
        /// <param name="breed">Порода питомца</param>
        /// <param name="ownerId">Идентификатор владельца</param>
        /// <returns>Созданный объект питомца</returns>
        public PetDto CreatePet(string name, string species, string breed, Guid ownerId)
        {
            if (!_ownerManager.OwnerExists(ownerId))
                throw new ArgumentException("Владелец не найден");

            var pet = new PetDto
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Species = species.Trim(),
                Breed = breed.Trim(),
                OwnerId = ownerId
            };

            _pets.Add(pet);
            return pet;
        }

        /// <summary>
        /// Возвращает всех питомцев
        /// </summary>
        /// <returns>Список всех питомцев</returns>
        public List<PetDto> GetAllPets() => _pets;

        /// <summary>
        /// Возвращает питомцев по владельцу
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца</param>
        /// <returns>Список питомцев владельца</returns>
        public List<PetDto> GetPetsByOwner(Guid ownerId) =>
            _pets.Where(p => p.OwnerId == ownerId).ToList();

        /// <summary>
        /// Находит питомца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор питомца</param>
        /// <returns>Найденный питомец или null</returns>
        public PetDto GetPetById(Guid id) => _pets.FirstOrDefault(p => p.Id == id);

        /// <summary>
        /// Обновляет данные питомца
        /// </summary>
        /// <param name="petDto">Объект с обновленными данными питомца</param>
        public void UpdatePet(PetDto petDto)
        {
            var existing = GetPetById(petDto.Id);
            if (existing == null)
                throw new ArgumentException("Питомец не найден");

            existing.Name = petDto.Name.Trim();
            existing.Species = petDto.Species.Trim();
            existing.Breed = petDto.Breed.Trim();
            existing.OwnerId = petDto.OwnerId;
        }

        /// <summary>
        /// Удаляет питомца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемого питомца</param>
        public void DeletePet(Guid id)
        {
            var pet = GetPetById(id);
            if (pet != null)
                _pets.Remove(pet);
            else
                throw new ArgumentException("Питомец не найден");
        }
    }
}