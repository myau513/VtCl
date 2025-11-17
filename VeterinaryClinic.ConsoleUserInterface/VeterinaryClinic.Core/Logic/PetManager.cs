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

        public PetManager(OwnerManager ownerManager)
        {
            _ownerManager = ownerManager;
        }

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

        public List<PetDto> GetAllPets() => _pets;

        public List<PetDto> GetPetsByOwner(Guid ownerId) =>
            _pets.Where(p => p.OwnerId == ownerId).ToList();

        public PetDto GetPetById(Guid id) => _pets.FirstOrDefault(p => p.Id == id);

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