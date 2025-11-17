using VeterinaryClinic.Core.DTO;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Mapper
{
    public static class AppointmentMapper
    {


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