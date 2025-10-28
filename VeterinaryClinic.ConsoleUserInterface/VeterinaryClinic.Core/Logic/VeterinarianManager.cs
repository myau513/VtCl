using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Essence;

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

        /// <summary>
        /// Создает менеджер ветеринаров и автоматически инициализирует базовых ветеринаров
        /// </summary>
        public VeterinarianManager()
        {
            InitializeVeterinarians();
        }

        /// <summary>
        /// Инициализирует базовый список ветеринаров с их рабочими днями
        /// </summary>
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

        /// <summary>
        /// Получает список всех ветеринаров клиники
        /// </summary>
        /// <returns>Список всех ветеринаров</returns>
        public List<Veterinarian> GetAllVeterinarians()
        {
            return veterinarians;
        }

        /// <summary>
        /// Находит ветеринара по ID
        /// </summary>
        /// <param name="id">ID ветеринара</param>
        /// <returns>Найденный ветеринар или null</returns>
        public Veterinarian GetVeterinarian(int id)
        {
            return veterinarians.FirstOrDefault(v => v.Id == id);
        }

        /// <summary>
        /// Находит ветеринара, работающего в указанный день недели
        /// </summary>
        /// <param name="day">День недели</param>
        /// <returns>Ветеринар, работающий в этот день</returns>
        public Veterinarian GetVeterinarianByDay(DayOfWeek day)
        {
            return veterinarians.FirstOrDefault(v => v.WorkDays.Contains(day));
        }

        /// <summary>
        /// Проверяет доступность временного слота для ветеринара
        /// </summary>
        /// <param name="veterinarianId">ID ветеринара</param>
        /// <param name="dateTime">Дата и время для проверки</param>
        /// <returns>True если слот доступен, иначе False</returns>
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

        /// <summary>
        /// Создает новую запись на прием к ветеринару
        /// </summary>
        /// <param name="petId">ID питомца</param>
        /// <param name="veterinarianId">ID ветеринара</param>
        /// <param name="appointmentDate">Дата приема</param>
        /// <param name="timeSlot">Временной слот (формат "10-00")</param>
        /// <param name="reason">Причина визита</param>
        /// <param name="breed">Порода питомца</param>
        /// <returns>Созданная запись на прием</returns>
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

        /// <summary>
        /// Добавляет запись в историю посещений
        /// </summary>
        /// <param name="petId">ID питомца</param>
        /// <param name="veterinarianName">Имя ветеринара</param>
        /// <param name="visitDate">Дата посещения</param>
        /// <param name="reason">Причина визита</param>
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

        /// <summary>
        /// Получает историю посещений для указанного питомца
        /// </summary>
        /// <param name="petId">ID питомца</param>
        /// <returns>Список записей истории посещений (от новых к старым)</returns>
        public List<VisitHistory> GetVisitHistoryByPet(int petId)
        {
            return visitHistory
                .Where(h => h.PetId == petId)
                .OrderByDescending(h => h.VisitDate)
                .ToList();
        }

        /// <summary>
        /// Получает все записи на прием на указанную дату
        /// </summary>
        /// <param name="date">Дата для поиска записей</param>
        /// <returns>Список записей на указанную дату</returns>
        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return appointments.Where(a => a.AppointmentDate.Date == date.Date).ToList();
        }

        /// <summary>
        /// Получает все записи на прием в клинике
        /// </summary>
        /// <returns>Список всех записей на прием</returns>
        public List<Appointment> GetAllAppointments()
        {
            return appointments;
        }

        /// <summary>
        /// Получает все записи на прием для указанного питомца
        /// </summary>
        /// <param name="petId">ID питомца</param>
        /// <returns>Список записей для питомца</returns>
        public List<Appointment> GetAppointmentsByPetId(int petId)
        {
            return appointments.Where(a => a.PetId == petId).ToList();
        }
    }
}