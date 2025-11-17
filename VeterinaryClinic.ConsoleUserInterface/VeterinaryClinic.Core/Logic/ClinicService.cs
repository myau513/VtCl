using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.DTO;
using VeterinaryClinic.Core.Essence;
using VeterinaryClinic.Core.Mapper;

namespace VeterinaryClinic.Core.Logic
{
    public class ClinicService
    {
        private readonly OwnerManager _ownerManager;
        private readonly PetManager _petManager;
        private readonly VeterinarianManager _vetManager;
        private readonly AppointmentManager _appointmentManager;
        private readonly VisitHistoryManager _visitHistoryManager;

        public ClinicService()
        {
            _ownerManager = new OwnerManager();
            _petManager = new PetManager(_ownerManager);
            _vetManager = new VeterinarianManager();
            _appointmentManager = new AppointmentManager();
            _visitHistoryManager = new VisitHistoryManager();
        }

        // === Владельцы ===
        public Owner CreateOwner(string fullName, string phoneNumber)
        {
            var ownerDto = _ownerManager.CreateOwner(fullName, phoneNumber);
            return OwnerMapper.ToDomain(ownerDto);
        }

        public List<Owner> FindOwnersByName(string name)
        {
            var allOwners = _ownerManager.GetAllOwners();
            var filteredOwners = allOwners
                .Where(o => o.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            return filteredOwners.Select(OwnerMapper.ToDomain).ToList();
        }

        public List<Owner> GetAllOwners()
        {
            return _ownerManager.GetAllOwners()
                .Select(OwnerMapper.ToDomain)
                .ToList();
        }

        public void UpdateOwner(Owner owner)
        {
            var ownerDto = OwnerMapper.ToDto(owner);
            _ownerManager.UpdateOwner(ownerDto);
        }

        public bool DeleteOwner(Guid ownerId)
        {
            try
            {
                _ownerManager.DeleteOwner(ownerId);
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
            var petDto = _petManager.CreatePet(name, species, breed, ownerId);
            return PetMapper.ToDomain(petDto);
        }

        public List<Pet> GetPetsByOwnerId(Guid ownerId)
        {
            return _petManager.GetPetsByOwner(ownerId)
                .Select(PetMapper.ToDomain)
                .ToList();
        }

        public List<Pet> GetAllPets()
        {
            return _petManager.GetAllPets()
                .Select(PetMapper.ToDomain)
                .ToList();
        }

        public void UpdatePet(Pet pet)
        {
            var petDto = PetMapper.ToDto(pet);
            _petManager.UpdatePet(petDto);
        }

        public bool DeletePet(Guid petId)
        {
            try
            {
                _petManager.DeletePet(petId);
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
            var allVets = _vetManager.GetAllVeterinarians();
            var vetDto = allVets.FirstOrDefault(v =>
                v.WorkDaysString.Split(',').Contains(day.ToString()));

            if (vetDto == null) return null;

            return VeterinarianMapper.ToDomain(vetDto);
        }

        public List<Veterinarian> GetAllVeterinarians()
        {
            return _vetManager.GetAllVeterinarians()
                .Select(VeterinarianMapper.ToDomain)
                .ToList();
        }

        // === Комплексные бизнес-методы ===
        public bool DeleteOwnerWithPets(Guid ownerId)
        {
            var pets = GetPetsByOwnerId(ownerId);
            foreach (var pet in pets)
            {
                DeletePet(pet.Id);
            }
            return DeleteOwner(ownerId);
        }

        public List<DateTime> GetAvailableTimeSlots(Guid vetId, DateTime date)
        {
            var availableSlots = new List<DateTime>();
            for (int hour = 10; hour < 19; hour++)
            {
                var slot = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                if (_appointmentManager.IsTimeSlotAvailable(vetId, slot))
                    availableSlots.Add(slot);
            }
            return availableSlots;
        }

        public Appointment CreateAppointment(Guid petId, Guid vetId, DateTime appointmentDate,
                                       string timeSlot, string reason, string breed)
        {
            var appointmentDto = _appointmentManager.CreateAppointment(petId, vetId, appointmentDate, timeSlot, reason, breed);

            // Создаем запись в истории визитов
            var vet = GetVeterinarian(vetId);
            _visitHistoryManager.CreateVisitHistory(petId, vet.FullName, appointmentDate, reason);

            return AppointmentMapper.ToDomain(appointmentDto);
        }

        // === Вспомогательные методы ===
        public Pet GetPet(Guid petId)
        {
            var petDto = _petManager.GetPetById(petId);
            return petDto != null ? PetMapper.ToDomain(petDto) : null;
        }

        public Veterinarian GetVeterinarian(Guid vetId)
        {
            var vetDto = _vetManager.GetVeterinarianById(vetId);
            return vetDto != null ? VeterinarianMapper.ToDomain(vetDto) : null;
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _appointmentManager.GetAppointmentsByDate(date)
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
            if (owners.Count == 0)
                return new List<VisitHistory>();

            var owner = owners[0];
            var pets = GetPetsByOwnerId(owner.Id);

            var allHistory = new List<VisitHistory>();

            foreach (var pet in pets)
            {
                var petHistory = _visitHistoryManager.GetVisitHistoryByPet(pet.Id)
                    .Select(VisitHistoryMapper.ToDomain)
                    .ToList();
                allHistory.AddRange(petHistory);
            }

            return allHistory.OrderByDescending(h => h.VisitDate).ToList();
        }
    }

    // Класс для данных расписания
    public class ScheduleData
    {
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<Veterinarian> Veterinarians { get; set; } = new List<Veterinarian>();
    }
}