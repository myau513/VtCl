using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class VisitHistoryManager
    {
        private List<VisitHistory> visitHistory = new List<VisitHistory>();
        private int nextHistoryId = 1;

        public void AddToVisitHistory(int petId, string veterinarianName, DateTime visitDate, string reason)
        {
            var historyRecord = new VisitHistory(
                nextHistoryId++,
                petId,
                veterinarianName,
                visitDate,
                reason
            );
            visitHistory.Add(historyRecord);
        }

        public List<VisitHistory> GetVisitHistoryByPet(int petId) =>
            visitHistory
                .Where(h => h.PetId == petId)
                .OrderByDescending(h => h.VisitDate)
                .ToList();
    }
}
