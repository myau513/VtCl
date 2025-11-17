using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class AppointmentManager
    {
        private List<AppointmentDto> _appointments = new List<AppointmentDto>();

        /// <summary>
        /// Проверяет доступность временного слота для ветеринара.
        /// </summary>
        /// <param name="veterinarianId">Идентификатор ветеринара</param>
        /// <param name="dateTime">Дата и время для проверки</param>
        /// <returns>True если временной слот доступен, иначе False</returns>
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

        /// <summary>
        /// Создает новую запись на прием.
        /// </summary>
        /// <param name="petId">Идентификатор животного</param>
        /// <param name="veterinarianId">Идентификатор ветеринара</param>
        /// <param name="appointmentDate">Дата приема</param>
        /// <param name="timeSlot">Временной слот</param>
        /// <param name="reason">Причина визита</param>
        /// <param name="breed">Порода животного</param>
        /// <returns>Созданная запись на прием</returns>
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

        /// <summary>
        /// Получает все записи на прием по указанной дате.
        /// </summary>
        /// <param name="date">Дата для поиска записей</param>
        /// <returns>Список записей на указанную дату</returns>
        public List<AppointmentDto> GetAppointmentsByDate(DateTime date) =>
            _appointments.Where(a => a.AppointmentDate.Date == date.Date).ToList();

        /// <summary>
        /// Получает все записи на прием.
        /// </summary>
        /// <returns>Список всех записей</returns>
        public List<AppointmentDto> GetAllAppointments() => _appointments;

        /// <summary>
        /// Получает все записи на прием для указанного животного.
        /// </summary>
        /// <param name="petId">Идентификатор животного</param>
        /// <returns>Список записей для указанного животного</returns>
        public List<AppointmentDto> GetAppointmentsByPetId(Guid petId) =>
            _appointments.Where(a => a.PetId == petId).ToList(); // Исправлено с _pets на _appointments
    }
}