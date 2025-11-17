using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Essence
{
    public class AppointmentDto : IDomainObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PetId { get; set; }
        public Guid VeterinarianId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; }
        public string Reason { get; set; }
        public string Breed { get; set; }
    }
}
