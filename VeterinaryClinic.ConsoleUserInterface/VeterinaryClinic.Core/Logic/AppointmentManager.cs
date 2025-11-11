using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class AppointmentManager
    {
        private List<Appointment> appointments = new List<Appointment>();
        private int nextAppointmentId = 1;

        public bool IsTimeSlotAvailable(int veterinarianId, DateTime dateTime)
        {
            var normalizedDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);

            if (normalizedDateTime.Hour < 10 || normalizedDateTime.Hour >= 19)
                return false;

            return !appointments.Any(a =>
            {
                var normalizedAppointmentDate = new DateTime(a.AppointmentDate.Year, a.AppointmentDate.Month, a.AppointmentDate.Day, a.AppointmentDate.Hour, 0, 0);
                return a.VeterinarianId == veterinarianId &&
                       normalizedAppointmentDate == normalizedDateTime;
            });
        }

        public Appointment CreateAppointment(int petId, int veterinarianId, DateTime appointmentDate, string timeSlot,
                                           string reason, string breed)
        {
            if (!IsTimeSlotAvailable(veterinarianId, appointmentDate))
                throw new ArgumentException("Данное время уже занято или недоступно");

            var appointment = new Appointment(nextAppointmentId++, petId, veterinarianId,
                                            appointmentDate, timeSlot, reason, breed);
            appointments.Add(appointment);
            return appointment;
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date) =>
            appointments.Where(a => a.AppointmentDate.Date == date.Date).ToList();

        public List<Appointment> GetAllAppointments() => appointments;

        public List<Appointment> GetAppointmentsByPetId(int petId) =>
            appointments.Where(a => a.PetId == petId).ToList();
    }
}
