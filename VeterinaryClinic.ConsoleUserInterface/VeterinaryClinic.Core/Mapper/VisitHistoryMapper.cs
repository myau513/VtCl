using VeterinaryClinic.Core.Essence;
using Dto.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class VisitHistoryMapper
    {
        /// <summary>
        /// Преобразует доменную сущность истории посещений в DTO
        /// </summary>
        /// <param name="entity">Доменная сущность истории посещений</param>
        /// <returns>DTO истории посещений</returns>
        public static VisitHistoryDto ToDto(this VisitHistory entity)
        {
            return new VisitHistoryDto
            {
                Id = entity.Id,
                PetId = entity.PetId,
                VeterinarianName = entity.VeterinarianName,
                VisitDate = entity.VisitDate,
                Reason = entity.Reason,
                Diagnosis = entity.Diagnosis,
                Treatment = entity.Treatment,
                Notes = entity.Notes
            };
        }

        /// <summary>
        /// Преобразует DTO истории посещений в доменную сущность
        /// </summary>
        /// <param name="dto">DTO истории посещений</param>
        /// <returns>Доменная сущность истории посещений</returns>
        public static VisitHistory ToDomain(VisitHistoryDto dto) =>
            new VisitHistory(dto.Id, dto.PetId, dto.VeterinarianName, dto.VisitDate, dto.Reason, dto.Diagnosis, dto.Treatment, dto.Notes);
    }
}