using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Essence
{
    public class VisitHistory : IDomainObject
    {
        public Guid Id_db { get; set; } = Guid.NewGuid();
        public int Id { get; set; }
        public int PetId { get; set; }
        public string VeterinarianName { get; set; }
        public DateTime VisitDate { get; set; }
        public string Reason { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Notes { get; set; }

        public VisitHistory(int id, int petId, string veterinarianName, DateTime visitDate,
                          string reason, string diagnosis = "", string treatment = "", string notes = "")
        {
            Id = id;
            PetId = petId;
            VeterinarianName = veterinarianName;
            VisitDate = visitDate;
            Reason = reason;
            Diagnosis = diagnosis;
            Treatment = treatment;
            Notes = notes;
        }
    }
}
