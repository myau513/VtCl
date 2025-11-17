using VeterinaryClinic.Core.DTO;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class OwnerMapper
    {
       

        public static OwnerDto ToDto(this Owner entity)
        {
            return new OwnerDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                PhoneNumber = entity.PhoneNumber
            };
        }
        public static Owner ToDomain(OwnerDto dto) => new Owner
        {
            Id = dto.Id,
            FullName = dto.FullName,
            PhoneNumber = dto.PhoneNumber
        };
    }
}