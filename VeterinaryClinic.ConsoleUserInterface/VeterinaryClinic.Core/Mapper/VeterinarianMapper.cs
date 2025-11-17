using VeterinaryClinic.Core.Essence;
using Dto.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class VeterinarianMapper
    {
        /// <summary>
        /// Преобразует доменную сущность ветеринара в DTO
        /// </summary>
        /// <param name="entity">Доменная сущность ветеринара</param>
        /// <returns>DTO ветеринара</returns>
        public static VeterinarianDto ToDto(this Veterinarian entity)
        {
            return new VeterinarianDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                WorkDaysString = entity.WorkDaysString
            };
        }

        /// <summary>
        /// Преобразует DTO ветеринара в доменную сущность
        /// </summary>
        /// <param name="dto">DTO ветеринара</param>
        /// <returns>Доменная сущность ветеринара</returns>
        public static Veterinarian ToDomain(VeterinarianDto dto) =>
            new Veterinarian(dto.Id, dto.FullName, dto.WorkDaysString);
    }
}