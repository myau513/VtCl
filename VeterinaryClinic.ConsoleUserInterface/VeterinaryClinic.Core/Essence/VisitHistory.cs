using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Essence
{
    public class VisitHistory : IDomainObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PetId { get; set; }
        public string VeterinarianName { get; set; }
        public DateTime VisitDate { get; set; }
        public string Reason { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string Notes { get; set; }
    }
}
