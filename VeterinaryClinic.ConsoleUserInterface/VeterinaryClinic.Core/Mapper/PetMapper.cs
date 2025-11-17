using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.DTO;
using VeterinaryClinic.Core.Essence;

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
        public static Pet ToDomain(PetDto dto) => new Pet
        {
            Id = dto.Id,
            Name = dto.Name,
            Species = dto.Species,
            Breed = dto.Breed,
            OwnerId = dto.OwnerId

        };
    }
}
