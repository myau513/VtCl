using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.DTO;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class VisitHistoryMapper
    {

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

        public static VisitHistory ToDomain(VisitHistoryDto dto) => new VisitHistory
        {
            Id = dto.Id,
            PetId = dto.PetId,
            VeterinarianName = dto.VeterinarianName,
            VisitDate = dto.VisitDate,
            Reason = dto.Reason,
            Diagnosis = dto.Diagnosis,
            Treatment = dto.Treatment,
            Notes = dto.Notes
        };
    }
}
