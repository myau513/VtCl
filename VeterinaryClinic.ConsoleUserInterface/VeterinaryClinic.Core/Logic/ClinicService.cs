using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;
using Dto.Repo;
using VeterinaryClinic.Core.Essence;
using VeterinaryClinic.Core.Mapper;

namespace VeterinaryClinic.Core.Logic
{
    public class ClinicService
    {
        private readonly IRepositoryDto<OwnerDto> _ownerRepository;
        private readonly IRepositoryDto<PetDto> _petRepository;
        private readonly IRepositoryDto<VeterinarianDto> _vetRepository;
        private readonly IRepositoryDto<AppointmentDto> _appointmentRepository;
        private readonly IRepositoryDto<VisitHistoryDto> _visitHistoryRepository;

        public ClinicService(
            IRepositoryDto<OwnerDto> ownerRepository,
            IRepositoryDto<PetDto> petRepository,
            IRepositoryDto<VeterinarianDto> vetRepository,
            IRepositoryDto<AppointmentDto> appointmentRepository,
            IRepositoryDto<VisitHistoryDto> visitHistoryRepository)
        {
            _ownerRepository = ownerRepository;
            _petRepository = petRepository;
            _vetRepository = vetRepository;
            _appointmentRepository = appointmentRepository;
            _visitHistoryRepository = visitHistoryRepository;
        }

        // === Владельцы ===

        public Owner CreateOwner(string fullName, string phoneNumber)
        {
            var owner = new Owner(Guid.NewGuid(), fullName, phoneNumber);
            var ownerDto = OwnerMapper.ToDto(owner);
            _ownerRepository.Add(ownerDto);
            _ownerRepository.Save();
            return owner;
        }

        public List<Owner> FindOwnersByName(string name)
        {
            var allOwners = _ownerRepository.GetAll();
            var filteredOwners = allOwners.Where(o =>
                !string.IsNullOrEmpty(o.FullName) &&
                o.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
            return filteredOwners.Select(OwnerMapper.ToDomain).ToList();
        }


        public List<Owner> GetAllOwners()
        {
            var allOwners = _ownerRepository.GetAll();
            return allOwners.Select(OwnerMapper.ToDomain).ToList();
        }

        public void UpdateOwner(Owner owner)
        {
            var dto = OwnerMapper.ToDto(owner);
            _ownerRepository.Update(dto);
            _ownerRepository.Save();
        }

        public bool DeleteOwner(Guid ownerId)
        {
            try
            {
                _ownerRepository.Delete(ownerId);
                _ownerRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // === Питомцы ===

        public Pet CreatePet(string name, string species, string breed, Guid ownerId)
        {
            var pet = new Pet(Guid.NewGuid(), name, species, breed, ownerId);
            var dto = PetMapper.ToDto(pet);
            _petRepository.Add(dto);
            _petRepository.Save();
            return pet;
        }

        public List<Pet> GetPetsByOwnerId(Guid ownerId)
        {
            var pets = _petRepository.GetAll().Where(p => p.OwnerId == ownerId);
            return pets.Select(PetMapper.ToDomain).ToList();
        }

        public List<Pet> GetAllPets()
        {
            return _petRepository.GetAll()
                .Select(PetMapper.ToDomain)
                .ToList();
        }

        public void UpdatePet(Pet pet)
        {
            var dto = PetMapper.ToDto(pet);
            _petRepository.Update(dto);
            _petRepository.Save();
        }

        public bool DeletePet(Guid petId)
        {
            try
            {
                _petRepository.Delete(petId);
                _petRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // === Ветеринары ===

        public Veterinarian GetVeterinarianByDay(DayOfWeek day)
        {
            var allVets = _vetRepository.GetAll();
            var vetDto = allVets.FirstOrDefault(v => v.WorkDaysString.Split(',').Contains(day.ToString()));
            return vetDto == null ? null : VeterinarianMapper.ToDomain(vetDto);
        }

        public List<Veterinarian> GetAllVeterinarians()
        {
            return _vetRepository.GetAll()
                .Select(VeterinarianMapper.ToDomain)
                .ToList();
        }

        // === Комплексные бизнес-методы ===

        public bool DeleteOwnerWithPets(Guid ownerId)
        {
            var pets = GetPetsByOwnerId(ownerId);
            foreach (var pet in pets)
                DeletePet(pet.Id);

            return DeleteOwner(ownerId);
        }

        public List<DateTime> GetAvailableTimeSlots(Guid vetId, DateTime date)
        {
            var availableSlots = new List<DateTime>();
            for (int hour = 10; hour < 19; hour++)
            {
                var slot = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                if (IsTimeSlotAvailable(vetId, slot))
                    availableSlots.Add(slot);
            }
            return availableSlots;
        }

        private bool IsTimeSlotAvailable(Guid vetId, DateTime slot)
        {
            var appointments = _appointmentRepository.GetAll()
                .Where(a => a.VeterinarianId == vetId && a.AppointmentDate == slot.Date && a.TimeSlot == slot.ToString("HH:mm"));
            return !appointments.Any();
        }

        public Appointment CreateAppointment(Guid petId, Guid vetId, DateTime appointmentDate,
                                     string timeSlot, string reason, string breed,
                                     string diagnosis, string treatment, string notes)
        {
            var appointment = new Appointment(Guid.NewGuid(), petId, vetId, appointmentDate, timeSlot, reason, breed);
            var appointmentDto = AppointmentMapper.ToDto(appointment);
            _appointmentRepository.Add(appointmentDto);
            _appointmentRepository.Save();

            var vet = GetVeterinarian(vetId);
            var visitHistory = new VisitHistory(Guid.NewGuid(), petId, vet.FullName, appointmentDate, reason, diagnosis, treatment, notes);
            var visitHistoryDto = VisitHistoryMapper.ToDto(visitHistory);
            _visitHistoryRepository.Add(visitHistoryDto);
            _visitHistoryRepository.Save();

            return appointment;
        }

        // === Вспомогательные методы ===

        public Pet GetPet(Guid petId)
        {
            var petDto = _petRepository.GetById(petId);
            return petDto == null ? null : PetMapper.ToDomain(petDto);
        }

        public Veterinarian GetVeterinarian(Guid vetId)
        {
            var vetDto = _vetRepository.GetById(vetId);
            return vetDto == null ? null : VeterinarianMapper.ToDomain(vetDto);
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _appointmentRepository.GetAll()
                .Where(a => a.AppointmentDate.Date == date.Date)
                .Select(AppointmentMapper.ToDomain)
                .ToList();
        }

        // === Методы для работы с расписанием и историей ===

        public ScheduleData GetScheduleData(DateTime date)
        {
            var appointments = GetAppointmentsByDate(date);
            var veterinarians = GetAllVeterinarians();

            return new ScheduleData
            {
                Appointments = appointments,
                Veterinarians = veterinarians
            };
        }

        public List<VisitHistory> GetVisitHistoryByOwner(string ownerName)
        {
            var owners = FindOwnersByName(ownerName);
            if (!owners.Any())
                return new List<VisitHistory>();

            var owner = owners.First();
            var pets = GetPetsByOwnerId(owner.Id);

            var allHistory = new List<VisitHistory>();
            foreach (var pet in pets)
            {
                var petHistory = _visitHistoryRepository.GetAll()
                    .Where(h => h.PetId == pet.Id)
                    .Select(VisitHistoryMapper.ToDomain)
                    .ToList();

                allHistory.AddRange(petHistory);
            }

            return allHistory.OrderByDescending(h => h.VisitDate).ToList();
        }
    }

    public class ScheduleData
    {
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<Veterinarian> Veterinarians { get; set; } = new List<Veterinarian>();
    }
}
