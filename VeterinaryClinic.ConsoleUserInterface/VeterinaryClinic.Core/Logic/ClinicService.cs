using System;
using System.Collections.Generic;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.Core.Logic
{
    public class ClinicService
    {
        private readonly OwnerManager ownerManager;
        private readonly PetManager petManager;
        private readonly VeterinarianManager vetManager;

        public ClinicService(OwnerManager ownerManager, PetManager petManager, VeterinarianManager vetManager)
        {
            this.ownerManager = ownerManager;
            this.petManager = petManager;
            this.vetManager = vetManager;
        }

        // Владельцы
        public Owner CreateOwner(string fullName, string phoneNumber) =>
            ownerManager.CreateOwner(fullName, phoneNumber);

        public List<Owner> FindOwnersByName(string name) =>
            ownerManager.FindOwnersByName(name);

        public bool DeleteOwnerWithPets(int ownerId)
        {
            var pets = petManager.GetPetsByOwnerId(ownerId);
            foreach (var pet in pets)
            {
                petManager.DeletePet(pet.Id);
            }
            return ownerManager.DeleteOwner(ownerId);
        }

        public List<Owner> GetAllOwners() =>
            ownerManager.GetAllOwners();

        // Питомцы
        public Pet CreatePet(string name, string species, string breed, int ownerId) =>
            petManager.CreatePet(name, species, breed, ownerId);

        public List<Pet> GetPetsByOwnerId(int ownerId) =>
            petManager.GetPetsByOwnerId(ownerId);

        public bool DeletePet(int petId) =>
            petManager.DeletePet(petId);

        public List<Pet> GetAllPets() =>
            petManager.GetAllPets();

        // Ветеринары и записи
        public Veterinarian GetVeterinarianByDay(DayOfWeek day) =>
            vetManager.GetVeterinarianByDay(day);

        public List<Veterinarian> GetAllVeterinarians() =>
            vetManager.GetAllVeterinarians();

        public List<DateTime> GetAvailableTimeSlots(int vetId, DateTime date)
        {
            var availableSlots = new List<DateTime>();
            for (int hour = 10; hour < 19; hour++)
            {
                var slot = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                if (vetManager.IsTimeSlotAvailable(vetId, slot))
                    availableSlots.Add(slot);
            }
            return availableSlots;
        }

        public Appointment CreateAppointment(int petId, int vetId, DateTime appointmentDate, string timeSlot, string reason, string breed) =>
            vetManager.CreateAppointment(petId, vetId, appointmentDate, timeSlot, reason, breed);

        public List<VisitHistory> GetVisitHistoryByPet(int petId) =>
            vetManager.GetVisitHistoryByPet(petId);

        public List<Appointment> GetAppointmentsByDate(DateTime date) =>
            vetManager.GetAppointmentsByDate(date);

        public void UpdateOwner(Owner owner)
        {
            ownerManager.UpdateOwner(owner);
        }

        public bool DeleteOwner(int ownerId)
        {
            return ownerManager.DeleteOwner(ownerId);
        }

        public void UpdatePet(Pet pet)
        {
            petManager.UpdatePet(pet);
        }

    }
}
