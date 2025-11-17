using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.DTO;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class VeterinarianMapper
    {

        public static VeterinarianDto ToDto(this Veterinarian entity)
        {
            return new VeterinarianDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                WorkDaysString = entity.WorkDaysString
            };
        }

        public static Veterinarian ToDomain(VeterinarianDto dto) => new Veterinarian
        {
            Id = dto.Id,
            FullName = dto.FullName,
            WorkDaysString = dto.WorkDaysString
        };
    }
}
