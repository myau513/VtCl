using System;

namespace Dto.Essence
{ 
    public class VisitHistoryDto : IDomainObject
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