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
        private readonly IBaseRepository<OwnerDto> _ownerRepo;
        private readonly IBaseRepository<PetDto> _petRepo;
        private readonly IBaseRepository<VeterinarianDto> _vetRepo;
        private readonly IBaseRepository<AppointmentDto> _appointmentRepo;
        private readonly IBaseRepository<VisitHistoryDto> _visitHistoryRepo;

        public ClinicService(
            IBaseRepository<OwnerDto> ownerRepo,
            IBaseRepository<PetDto> petRepo,
            IBaseRepository<VeterinarianDto> vetRepo,
            IBaseRepository<AppointmentDto> appointmentRepo,
            IBaseRepository<VisitHistoryDto> visitHistoryRepo)
        {
            _ownerRepo = ownerRepo;
            _petRepo = petRepo;
            _vetRepo = vetRepo;
            _appointmentRepo = appointmentRepo;
            _visitHistoryRepo = visitHistoryRepo;
        }

        // === Владельцы ===

        public Owner CreateOwner(string fullName, string phoneNumber)
        {
            var owner = new Owner(Guid.NewGuid(), fullName, phoneNumber);
            var ownerDto = OwnerMapper.ToDto(owner);
            _ownerRepo.Add(ownerDto);
            _ownerRepo.Save();
            return owner;
        }

        public List<Owner> FindOwnersByName(string name)
        {
            var allOwners = _ownerRepo.GetAll();
            var filteredOwners = allOwners.Where(o =>
                !string.IsNullOrEmpty(o.FullName) &&
                o.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
            return filteredOwners.Select(OwnerMapper.ToDomain).ToList();
        }

        public List<Owner> GetAllOwners()
        {
            var allOwners = _ownerRepo.GetAll();
            return allOwners.Select(OwnerMapper.ToDomain).ToList();
        }

        public void UpdateOwner(Owner owner)
        {
            var dto = OwnerMapper.ToDto(owner);
            _ownerRepo.Update(dto);
            _ownerRepo.Save();
        }

        public bool DeleteOwner(Guid ownerId)
        {
            try
            {
                _ownerRepo.Delete(ownerId);
                _ownerRepo.Save();
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
            _petRepo.Add(dto);
            _petRepo.Save();
            return pet;
        }

        public List<Pet> GetPetsByOwnerId(Guid ownerId)
        {
            var pets = _petRepo.GetAll().Where(p => p.OwnerId == ownerId);
            return pets.Select(PetMapper.ToDomain).ToList();
        }

        public List<Pet> GetAllPets()
        {
            return _petRepo.GetAll()
                .Select(PetMapper.ToDomain)
                .ToList();
        }

        public void UpdatePet(Pet pet)
        {
            var dto = PetMapper.ToDto(pet);
            _petRepo.Update(dto);
            _petRepo.Save();
        }

        public bool DeletePet(Guid petId)
        {
            try
            {
                _petRepo.Delete(petId);
                _petRepo.Save();
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
            var allVets = _vetRepo.GetAll();
            var vetDto = allVets.FirstOrDefault(v =>
                !string.IsNullOrEmpty(v.WorkDaysString) &&
                v.WorkDaysString.Split(',').Contains(day.ToString()));
            return vetDto == null ? null : VeterinarianMapper.ToDomain(vetDto);
        }

        public List<Veterinarian> GetAllVeterinarians()
        {
            return _vetRepo.GetAll()
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
            var appointments = _appointmentRepo.GetAll()
                .Where(a => a.VeterinarianId == vetId &&
                           a.AppointmentDate.Date == slot.Date &&
                           a.TimeSlot == slot.ToString("HH:mm"));
            return !appointments.Any();
        }

        public Appointment CreateAppointment(Guid petId, Guid vetId, DateTime appointmentDate,
                                     string timeSlot, string reason, string breed,
                                     string diagnosis, string treatment, string notes)
        {
            var appointment = new Appointment(Guid.NewGuid(), petId, vetId, appointmentDate, timeSlot, reason, breed);
            var appointmentDto = AppointmentMapper.ToDto(appointment);
            _appointmentRepo.Add(appointmentDto);
            _appointmentRepo.Save();

            var vet = GetVeterinarian(vetId);
            var visitHistory = new VisitHistory(Guid.NewGuid(), petId, vet.FullName, appointmentDate, reason, diagnosis, treatment, notes);
            var visitHistoryDto = VisitHistoryMapper.ToDto(visitHistory);
            _visitHistoryRepo.Add(visitHistoryDto);
            _visitHistoryRepo.Save();

            return appointment;
        }

        // === Вспомогательные методы ===

        public Pet GetPet(Guid petId)
        {
            var petDto = _petRepo.GetById(petId);
            return petDto == null ? null : PetMapper.ToDomain(petDto);
        }

        public Veterinarian GetVeterinarian(Guid vetId)
        {
            var vetDto = _vetRepo.GetById(vetId);
            return vetDto == null ? null : VeterinarianMapper.ToDomain(vetDto);
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _appointmentRepo.GetAll()
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
                var petHistory = _visitHistoryRepo.GetAll()
                    .Where(h => h.PetId == pet.Id)
                    .Select(VisitHistoryMapper.ToDomain)
                    .ToList();

                allHistory.AddRange(petHistory);
            }

            return allHistory.OrderByDescending(h => h.VisitDate).ToList();
        }
        public bool DeleteAppointment(Guid appointmentId)
        {
            try
            {
                _appointmentRepo.Delete(appointmentId);
                _appointmentRepo.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении записи: {ex.Message}");
                return false;
            }
        }
    }

    public class ScheduleData
    {
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<Veterinarian> Veterinarians { get; set; } = new List<Veterinarian>();
    }
}