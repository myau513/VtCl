using VeterinaryClinic.Core.Essence;
using Dto.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class AppointmentMapper
    {
        /// <summary>
        /// Преобразует сущность Appointment в DTO.
        /// </summary>
        /// <param name="entity">Сущность Appointment</param>
        /// <returns>DTO объект AppointmentDto</returns>
        public static AppointmentDto ToDto(this Appointment entity)
        {
            return new AppointmentDto
            {
                Id = entity.Id,
                PetId = entity.PetId,
                VeterinarianId = entity.VeterinarianId,
                AppointmentDate = entity.AppointmentDate,
                TimeSlot = entity.TimeSlot,
                Reason = entity.Reason,
                Breed = entity.Breed
            };
        }

        /// <summary>
        /// Преобразует DTO AppointmentDto в сущность Appointment.
        /// </summary>
        /// <param name="dto">DTO объект AppointmentDto</param>
        /// <returns>Сущность Appointment</returns>
        public static Appointment ToDomain(AppointmentDto dto) => new Appointment
        {
            Id = dto.Id,
            PetId = dto.PetId,
            VeterinarianId = dto.VeterinarianId,
            AppointmentDate = dto.AppointmentDate,
            TimeSlot = dto.TimeSlot,
            Reason = dto.Reason,
            Breed = dto.Breed
        };
    }
}