using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.Core.Logic
{
    public class VeterinarianManager
    {
        private List<Veterinarian> veterinarians = new List<Veterinarian>();
        private List<Appointment> appointments = new List<Appointment>();
        private List<VisitHistory> visitHistory = new List<VisitHistory>();
        private int nextVetId = 1;
        private int nextAppointmentId = 1;
        private int nextHistoryId = 1;

        public VeterinarianManager()
        {
            InitializeVeterinarians();
        }

        private void InitializeVeterinarians()
        {
            // Иванов - пн, чт
            veterinarians.Add(new Veterinarian(nextVetId++, "Иванов И.И.",
                new[] { DayOfWeek.Monday, DayOfWeek.Thursday }));

            // Петров - вт, пт
            veterinarians.Add(new Veterinarian(nextVetId++, "Петров П.П.",
                new[] { DayOfWeek.Tuesday, DayOfWeek.Friday }));

            // Сидоров - ср, сб
            veterinarians.Add(new Veterinarian(nextVetId++, "Сидоров С.С.",
                new[] { DayOfWeek.Wednesday, DayOfWeek.Saturday }));
        }

        public List<Veterinarian> GetAllVeterinarians()
        {
            return veterinarians;
        }

        public Veterinarian GetVeterinarian(int id)
        {
            return veterinarians.FirstOrDefault(v => v.Id == id);
        }

        public Veterinarian GetVeterinarianByDay(DayOfWeek day)
        {
            return veterinarians.FirstOrDefault(v => v.WorkDays.Contains(day));
        }

        public bool IsTimeSlotAvailable(int veterinarianId, DateTime dateTime)
        {
            // Проверяем, что время "ровное" (10:00, 11:00 и т.д.)
            if (dateTime.Minute != 0 || dateTime.Second != 0)
                return false;

            // Проверяем рабочее время (10:00-19:00)
            if (dateTime.Hour < 10 || dateTime.Hour >= 19)
                return false;

            // Проверяем, не занято ли время
            return !appointments.Any(a =>
                a.VeterinarianId == veterinarianId &&
                a.AppointmentDate == dateTime);
        }

        public Appointment CreateAppointment(int petId, int veterinarianId, DateTime appointmentDate, string timeSlot,
                                           string reason, string breed)
        {
            if (!IsTimeSlotAvailable(veterinarianId, appointmentDate))
                throw new ArgumentException("Данное время уже занято или недоступно");

            var appointment = new Appointment(nextAppointmentId++, petId, veterinarianId,
                                            appointmentDate, timeSlot, reason, breed);
            appointments.Add(appointment);

            // Автоматически создаем запись в истории
            var vet = GetVeterinarian(veterinarianId);
            AddToVisitHistory(petId, vet.FullName, appointmentDate, reason);

            return appointment;
        }

        private void AddToVisitHistory(int petId, string veterinarianName, DateTime visitDate, string reason)
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

        public List<VisitHistory> GetVisitHistoryByPet(int petId)
        {
            return visitHistory
                .Where(h => h.PetId == petId)
                .OrderByDescending(h => h.VisitDate)
                .ToList();
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return appointments.Where(a => a.AppointmentDate.Date == date.Date).ToList();
        }
    }
}
