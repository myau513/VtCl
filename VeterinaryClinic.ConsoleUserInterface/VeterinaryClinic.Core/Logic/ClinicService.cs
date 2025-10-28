using System;
using System.Collections.Generic;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Essence;

namespace VeterinaryClinic.Core.Logic
{
    public class ClinicService
    {
        private readonly OwnerManager ownerManager;
        private readonly PetManager petManager;
        private readonly VeterinarianManager vetManager;

        /// <summary>
        /// Создает новый сервис клиники с менеджерами владельцев, питомцев и ветеринаров
        /// </summary>
        /// <param name="ownerManager">Менеджер для работы с владельцами</param>
        /// <param name="petManager">Менеджер для работы с питомцами</param>
        /// <param name="vetManager">Менеджер для работы с ветеринарами и записями</param>
        
        public ClinicService(OwnerManager ownerManager, PetManager petManager, VeterinarianManager vetManager)
        {
            this.ownerManager = ownerManager;
            this.petManager = petManager;
            this.vetManager = vetManager;
        }

        /// <summary>
        /// Создает нового владельца животного
        /// </summary>
        /// <param name="fullName">Полное имя владельца</param>
        /// <param name="phoneNumber">Номер телефона владельца</param>
        /// <returns>Созданный владелец</returns>
        
        public Owner CreateOwner(string fullName, string phoneNumber) =>
            ownerManager.CreateOwner(fullName, phoneNumber);

        /// <summary>
        /// Ищет владельцев по имени (частичное совпадение)
        /// </summary>
        /// <param name="name">Имя или часть имени для поиска</param>
        /// <returns>Список найденных владельцев</returns>

        public List<Owner> FindOwnersByName(string name) =>
            ownerManager.FindOwnersByName(name);

        /// <summary>
        /// Удаляет владельца и всех его питомцев
        /// </summary>
        /// <param name="ownerId">ID владельца для удаления</param>
        /// <returns>True если удаление успешно, иначе False</returns>
        
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
        /// Получает список всех владельцев
        /// </summary>
        /// <returns>Список всех владельцев в системе</returns>
        
        public List<Owner> GetAllOwners() =>
            ownerManager.GetAllOwners();

        /// <summary>
        /// Создает нового питомца
        /// </summary>
        /// <param name="name">Кличка питомца</param>
        /// <param name="species">Вид животного</param>
        /// <param name="breed">Порода животного</param>
        /// <param name="ownerId">ID владельца</param>
        /// <returns>Созданный питомец</returns>
        
        public Pet CreatePet(string name, string species, string breed, int ownerId) =>
            petManager.CreatePet(name, species, breed, ownerId);

        /// <summary>
        /// Получает всех питомцев указанного владельца
        /// </summary>
        /// <param name="ownerId">ID владельца</param>
        /// <returns>Список питомцев владельца</returns>

        public List<Pet> GetPetsByOwnerId(int ownerId) =>
            petManager.GetPetsByOwnerId(ownerId);

        /// <summary>
        /// Удаляет питомца по ID
        /// </summary>
        /// <param name="petId">ID питомца для удаления</param>
        /// <returns>True если удаление успешно, иначе False</returns>
        
        public bool DeletePet(int petId) =>
            petManager.DeletePet(petId);

        /// <summary>
        /// Получает список всех питомцев в клинике
        /// </summary>
        /// <returns>Список всех питомцев</returns>
        
        public List<Pet> GetAllPets() =>
            petManager.GetAllPets();

        /// <summary>
        /// Находит ветеринара, работающего в указанный день недели
        /// </summary>
        /// <param name="day">День недели</param>
        /// <returns>Ветеринар, работающий в этот день</returns>

        public Veterinarian GetVeterinarianByDay(DayOfWeek day) =>
            vetManager.GetVeterinarianByDay(day);

        /// <summary>
        /// Получает список всех ветеринаров клиники
        /// </summary>
        /// <returns>Список всех ветеринаров</returns>

        public List<Veterinarian> GetAllVeterinarians() =>
            vetManager.GetAllVeterinarians();

        /// <summary>
        /// Получает список доступных временных слотов для ветеринара на указанную дату
        /// </summary>
        /// <param name="vetId">ID ветеринара</param>
        /// <param name="date">Дата для проверки</param>
        /// <returns>Список доступных временных слотов (с 10:00 до 18:00)</returns>

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

        /// <summary>
        /// Создает новую запись на прием к ветеринару
        /// </summary>
        /// <param name="petId">ID питомца</param>
        /// <param name="vetId">ID ветеринара</param>
        /// <param name="appointmentDate">Дата приема</param>
        /// <param name="timeSlot">Временной слот (формат "10-00")</param>
        /// <param name="reason">Причина визита</param>
        /// <param name="breed">Порода питомца</param>
        /// <returns>Созданная запись на прием</returns>

        public Appointment CreateAppointment(int petId, int vetId, DateTime appointmentDate, string timeSlot, string reason, string breed) =>
            vetManager.CreateAppointment(petId, vetId, appointmentDate, timeSlot, reason, breed);

        // <summary>
        /// Получает историю посещений для указанного питомца
        /// </summary>
        /// <param name="petId">ID питомца</param>
        /// <returns>Список записей истории посещений</returns>
        
        public List<VisitHistory> GetVisitHistoryByPet(int petId) =>
            vetManager.GetVisitHistoryByPet(petId);

        /// <summary>
        /// Получает все записи на прием на указанную дату
        /// </summary>
        /// <param name="date">Дата для поиска записей</param>
        /// <returns>Список записей на указанную дату</returns>
        
        public List<Appointment> GetAppointmentsByDate(DateTime date) =>
            vetManager.GetAppointmentsByDate(date);

        /// <summary>
        /// Обновляет информацию о владельце
        /// </summary>
        /// <param name="owner">Объект владельца с обновленными данными</param>

        public void UpdateOwner(Owner owner)
        {
            ownerManager.UpdateOwner(owner);
        }

        /// <summary>
        /// Удаляет владельца (без удаления его питомцев)
        /// </summary>
        /// <param name="ownerId">ID владельца для удаления</param>
        /// <returns>True если удаление успешно, иначе False</returns>
        
        public bool DeleteOwner(int ownerId)
        {
            return ownerManager.DeleteOwner(ownerId);
        }

        /// <summary>
        /// Обновляет информацию о питомце
        /// </summary>
        /// <param name="pet">Объект питомца с обновленными данными</param>
        
        public void UpdatePet(Pet pet)
        {
            petManager.UpdatePet(pet);
        }
        /// <summary>
        /// Получает все записи на прием в клинике
        /// </summary>
        /// <returns>Список всех записей на прием</returns>
        public List<Appointment> GetAllAppointments()
        {
            return vetManager.GetAllAppointments();
        }
        /// <summary>
        /// Получает все записи на прием для указанного питомца
        /// </summary>
        /// <param name="petId">ID питомца</param>
        /// <returns>Список записей для питомца</returns>
        public List<Appointment> GetAppointmentsByPetId(int petId)
        {
            return vetManager.GetAppointmentsByPetId(petId);
        }
    }
}
