using VeterinaryClinic.Core.Essence;
using Dto.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class OwnerMapper
    {
        /// <summary>
        /// Преобразует сущность Owner в DTO.
        /// </summary>
        /// <param name="entity">Сущность Owner</param>
        /// <returns>DTO объект OwnerDto</returns>
        public static OwnerDto ToDto(this Owner entity)
        {
            return new OwnerDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber
            };
        }

        /// <summary>
        /// Преобразует DTO OwnerDto в сущность Owner.
        /// </summary>
        /// <param name="dto">DTO объект OwnerDto</param>
        /// <returns>Сущность Owner</returns>
        public static Owner ToDomain(OwnerDto dto) =>
            new Owner(dto.Id, dto.FullName, dto.PhoneNumber);
    }
}