using VeterinaryClinic.Core.Essence;
using Dto.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class PetMapper
    {

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
        public static Pet ToDomain(PetDto dto) =>
        new Pet(dto.Id, dto.Name, dto.Species, dto.Breed, dto.OwnerId);

    }
}
