using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class VisitHistoryManager
    {
        private List<VisitHistoryDto> _visitHistory = new List<VisitHistoryDto>();

        public VisitHistoryDto CreateVisitHistory(Guid petId, string veterinarianName, DateTime visitDate,
                                                string reason, string diagnosis = "", string treatment = "", string notes = "")
        {
            var history = new VisitHistoryDto
            {
                Id = Guid.NewGuid(),
                PetId = petId, // Добавлен PetId
                VeterinarianName = veterinarianName,
                VisitDate = visitDate,
                Reason = reason,
                Diagnosis = diagnosis,
                Treatment = treatment,
                Notes = notes
            };

            _visitHistory.Add(history);
            return history;
        }

        public List<VisitHistoryDto> GetAllVisitHistories() => _visitHistory;

        public List<VisitHistoryDto> GetVisitHistoryByPet(Guid petId)
        {
            return _visitHistory.Where(v => v.PetId == petId)
                              .OrderByDescending(v => v.VisitDate)
                              .ToList();
        }
    }
}