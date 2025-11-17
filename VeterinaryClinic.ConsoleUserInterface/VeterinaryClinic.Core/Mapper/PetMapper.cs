using VeterinaryClinic.Core.Essence;
using Dto.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class PetMapper
    {
        /// <summary>
        /// Преобразует сущность Pet в DTO.
        /// </summary>
        /// <param name="entity">Сущность Pet</param>
        /// <returns>DTO объект PetDto</returns>
        public static PetDto ToDto(this Pet entity)
        {
            return new PetDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Species = entity.Species,
                Breed = entity.Breed,
                OwnerId = entity.OwnerId
            };
        }

        /// <summary>
        /// Преобразует DTO PetDto в сущность Pet.
        /// </summary>
        /// <param name="dto">DTO объект PetDto</param>
        /// <returns>Сущность Pet</returns>
        public static Pet ToDomain(PetDto dto) =>
            new Pet(dto.Id, dto.Name, dto.Species, dto.Breed, dto.OwnerId);
    }
}