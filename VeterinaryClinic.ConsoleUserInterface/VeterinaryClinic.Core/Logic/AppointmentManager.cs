using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.DTO;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class AppointmentManager
    {
        private List<AppointmentDto> _appointments = new List<AppointmentDto>();

        public bool IsTimeSlotAvailable(Guid veterinarianId, DateTime dateTime)
        {
            var normalizedDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);

            if (normalizedDateTime.Hour < 10 || normalizedDateTime.Hour >= 19)
                return false;

            return !_appointments.Any(a =>
            {
                var normalizedAppointmentDate = new DateTime(a.AppointmentDate.Year, a.AppointmentDate.Month, a.AppointmentDate.Day, a.AppointmentDate.Hour, 0, 0);
                return a.VeterinarianId == veterinarianId &&
                       normalizedAppointmentDate == normalizedDateTime;
            });
        }

        public AppointmentDto CreateAppointment(Guid petId, Guid veterinarianId, DateTime appointmentDate,
                                              string timeSlot, string reason, string breed)
        {
            if (!IsTimeSlotAvailable(veterinarianId, appointmentDate))
                throw new ArgumentException("Данное время уже занято или недоступно");

            var appointment = new AppointmentDto
            {
                Id = Guid.NewGuid(),
                PetId = petId,
                VeterinarianId = veterinarianId,
                AppointmentDate = appointmentDate,
                TimeSlot = timeSlot,
                Reason = reason,
                Breed = breed
            };

            _appointments.Add(appointment);
            return appointment;
        }

        public List<AppointmentDto> GetAppointmentsByDate(DateTime date) =>
            _appointments.Where(a => a.AppointmentDate.Date == date.Date).ToList();

        public List<AppointmentDto> GetAllAppointments() => _appointments;

        public List<AppointmentDto> GetAppointmentsByPetId(Guid petId) =>
            _appointments.Where(a => a.PetId == petId).ToList(); // Исправлено с _pets на _appointments
    }
}