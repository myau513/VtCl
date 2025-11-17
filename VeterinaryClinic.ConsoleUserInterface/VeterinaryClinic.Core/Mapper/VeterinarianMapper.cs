using VeterinaryClinic.Core.Essence;
using Dto.Essence;

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

        public static Veterinarian ToDomain(VeterinarianDto dto) =>
        new Veterinarian(dto.Id, dto.FullName, dto.WorkDaysString);

    }
}
