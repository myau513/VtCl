using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public int VeterinarianId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; }
        public string Reason { get; set; }
        public string Breed { get; set; }

        public Appointment(int id, int petId, int veterinarianId, DateTime appointmentDate, string timeSlot, string reason, string breed)
        {
            Id = id;
            PetId = petId;
            VeterinarianId = veterinarianId;
            AppointmentDate = appointmentDate;
            TimeSlot = timeSlot;
            Reason = reason;
            Breed = breed;
        }
    }
}
