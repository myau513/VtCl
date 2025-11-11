using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class ClinicService
    {
        private readonly OwnerManager ownerManager;
        private readonly PetManager petManager;
        private readonly VeterinarianManager vetManager;
        private readonly AppointmentManager appointmentManager;
        private readonly VisitHistoryManager visitHistoryManager;

        /// <summary>
        /// Создает новый сервис клиники с менеджерами владельцев, питомцев и ветеринаров
        /// </summary>
        /// <param name="ownerManager">Менеджер для работы с владельцами</param>
        /// <param name="petManager">Менеджер для работы с питомцами</param>
        /// <param name="vetManager">Менеджер для работы с ветеринарами</param>
        public ClinicService(OwnerManager ownerManager, PetManager petManager, VeterinarianManager vetManager)
        {
            this.ownerManager = ownerManager;
            this.petManager = petManager;
            this.vetManager = vetManager;
            this.appointmentManager = new AppointmentManager();
            this.visitHistoryManager = new VisitHistoryManager();
        }

        // === Базовые методы для владельцев ===

        public Owner CreateOwner(string fullName, string phoneNumber) =>
            ownerManager.CreateOwner(fullName, phoneNumber);

        public List<Owner> FindOwnersByName(string name) =>
            ownerManager.FindOwnersByName(name);

        public List<Owner> GetAllOwners() =>
            ownerManager.GetAllOwners();

        public void UpdateOwner(Owner owner) =>
            ownerManager.UpdateOwner(owner);

        public bool DeleteOwner(int ownerId) =>
            ownerManager.DeleteOwner(ownerId);

        // === Базовые методы для питомцев ===

        public Pet CreatePet(string name, string species, string breed, int ownerId) =>
            petManager.CreatePet(name, species, breed, ownerId);

        public List<Pet> GetPetsByOwnerId(int ownerId) =>
            petManager.GetPetsByOwnerId(ownerId);

        public List<Pet> GetAllPets() =>
            petManager.GetAllPets();

        public void UpdatePet(Pet pet) =>
            petManager.UpdatePet(pet);

        public bool DeletePet(int petId) =>
            petManager.DeletePet(petId);

        // === Базовые методы для ветеринаров ===

        public Veterinarian GetVeterinarianByDay(DayOfWeek day) =>
            vetManager.GetVeterinarianByDay(day);

        public List<Veterinarian> GetAllVeterinarians() =>
            vetManager.GetAllVeterinarians();

        // === Комплексные бизнес-методы (вынесенные из консольного приложения) ===

        /// <summary>
        /// Удаляет владельца и всех его питомцев
        /// </summary>
        public bool DeleteOwnerWithPets(int ownerId)
        {
            var pets = petManager.GetPetsByOwnerId(ownerId);
            foreach (var pet in pets)
            {
                petManager.DeletePet(pet.Id);
            }
            return ownerManager.DeleteOwner(ownerId);
        }

        /// <summary>
        /// Получает список доступных временных слотов для ветеринара на указанную дату
        /// </summary>
        public List<DateTime> GetAvailableTimeSlots(int vetId, DateTime date)
        {
            var availableSlots = new List<DateTime>();
            for (int hour = 10; hour < 19; hour++)
            {
                var slot = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                if (appointmentManager.IsTimeSlotAvailable(vetId, slot))
                    availableSlots.Add(slot);
            }
            return availableSlots;
        }

        /// <summary>
        /// Создает запись на прием с автоматической проверкой бизнес-правил
        /// </summary>
        public Appointment CreateAppointment(int petId, int vetId, DateTime appointmentDate, string timeSlot, string reason, string breed)
        {
            var appointment = appointmentManager.CreateAppointment(petId, vetId, appointmentDate, timeSlot, reason, breed);

            // Автоматически добавляем в историю посещений
            var vet = vetManager.GetVeterinarian(vetId);
            visitHistoryManager.AddToVisitHistory(petId, vet.FullName, appointmentDate, reason);

            return appointment;
        }

        /// <summary>
        /// Создает комплексную запись на прием с поиском по имени владельца
        /// </summary>
        public Appointment CreateAppointmentForOwner(string ownerName, DateTime date, string reason = "Плановый осмотр")
        {
            var owners = FindOwnersByName(ownerName);
            if (owners.Count == 0)
                throw new Exception("Владельцы не найдены.");

            if (owners.Count > 1)
                throw new Exception("Найдено несколько владельцев. Уточните поиск.");

            var selectedOwner = owners[0];
            var pets = GetPetsByOwnerId(selectedOwner.Id);

            if (pets.Count == 0)
                throw new Exception("У владельца нет питомцев.");

            if (pets.Count > 1)
                throw new Exception("У владельца несколько питомцев. Используйте метод с указанием petId.");

            var selectedPet = pets[0];
            return CreateAppointmentForPet(selectedPet.Id, date, reason);
        }

        /// <summary>
        /// Создает запись на прием для конкретного питомца с автоматическим подбором ветеринара
        /// </summary>
        public Appointment CreateAppointmentForPet(int petId, DateTime date, string reason = "Плановый осмотр")
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
                throw new Exception("Воскресенье - выходной.");

            var vet = GetVeterinarianByDay(date.DayOfWeek);
            if (vet == null)
                throw new Exception("Нет врачей в этот день.");

            var slots = GetAvailableTimeSlots(vet.Id, date);
            if (slots.Count == 0)
                throw new Exception("Нет свободных временных слотов.");

            var selectedTime = slots[0];
            var pet = petManager.GetPet(petId);

            return CreateAppointment(petId, vet.Id, selectedTime, selectedTime.ToString("HH:mm"), reason, pet.Breed);
        }

        /// <summary>
        /// Получает историю посещений для питомца по имени владельца
        /// </summary>
        public List<VisitHistory> GetVisitHistoryByOwner(string ownerName)
        {
            var owners = FindOwnersByName(ownerName);
            if (owners.Count == 0)
                throw new Exception("Владельцы не найдены.");

            if (owners.Count > 1)
                throw new Exception("Найдено несколько владельцев. Уточните поиск.");

            var selectedOwner = owners[0];
            var pets = GetPetsByOwnerId(selectedOwner.Id);

            if (pets.Count == 0)
                throw new Exception("У владельца нет питомцев.");

            if (pets.Count > 1)
                throw new Exception("У владельца несколько питомцев. Используйте метод с указанием petId.");

            return GetVisitHistoryByPet(pets[0].Id);
        }

        /// <summary>
        /// Получает историю посещений для указанного питомца
        /// </summary>
        public List<VisitHistory> GetVisitHistoryByPet(int petId) =>
            visitHistoryManager.GetVisitHistoryByPet(petId);

        /// <summary>
        /// Получает все записи на прием на указанную дату
        /// </summary>
        public List<Appointment> GetAppointmentsByDate(DateTime date) =>
            appointmentManager.GetAppointmentsByDate(date);

        /// <summary>
        /// Получает все записи на прием в клинике
        /// </summary>
        public List<Appointment> GetAllAppointments() =>
            appointmentManager.GetAllAppointments();

        /// <summary>
        /// Получает все записи на прием для указанного питомца
        /// </summary>
        public List<Appointment> GetAppointmentsByPetId(int petId) =>
            appointmentManager.GetAppointmentsByPetId(petId);

        /// <summary>
        /// Получает данные для отображения расписания на указанную дату
        /// </summary>
        public ScheduleData GetScheduleData(DateTime date)
        {
            return new ScheduleData
            {
                Date = date,
                Appointments = GetAppointmentsByDate(date),
                Veterinarians = GetAllVeterinarians()
            };
        }

        /// <summary>
        /// Получает информацию о питомце по ID
        /// </summary>
        public Pet GetPet(int petId) => petManager.GetPet(petId);

        /// <summary>
        /// Получает информацию о ветеринаре по ID
        /// </summary>
        public Veterinarian GetVeterinarian(int vetId) => vetManager.GetVeterinarian(vetId);
    }

    /// <summary>
    /// Вспомогательный класс для данных расписания
    /// </summary>
    public class ScheduleData
    {
        public DateTime Date { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Veterinarian> Veterinarians { get; set; }
    }
}