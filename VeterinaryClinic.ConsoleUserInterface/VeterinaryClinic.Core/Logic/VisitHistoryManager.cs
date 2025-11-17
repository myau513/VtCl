using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class VisitHistoryManager
    {
        private List<VisitHistoryDto> _visitHistory = new List<VisitHistoryDto>();

        /// <summary>
        /// Создает новую запись в истории посещений
        /// </summary>
        /// <param name="petId">Идентификатор питомца</param>
        /// <param name="veterinarianName">Имя ветеринара</param>
        /// <param name="visitDate">Дата посещения</param>
        /// <param name="reason">Причина посещения</param>
        /// <param name="diagnosis">Диагноз</param>
        /// <param name="treatment">Лечение</param>
        /// <param name="notes">Заметки</param>
        /// <returns>Созданная запись истории посещений</returns>
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

        /// <summary>
        /// Возвращает всю историю посещений
        /// </summary>
        /// <returns>Список всех записей истории посещений</returns>
        public List<VisitHistoryDto> GetAllVisitHistories() => _visitHistory;

        /// <summary>
        /// Возвращает историю посещений по питомцу
        /// </summary>
        /// <param name="petId">Идентификатор питомца</param>
        /// <returns>Отсортированный список записей истории посещений питомца</returns>
        public List<VisitHistoryDto> GetVisitHistoryByPet(Guid petId)
        {
            return _visitHistory.Where(v => v.PetId == petId)
                              .OrderByDescending(v => v.VisitDate)
                              .ToList();
        }
    }
}