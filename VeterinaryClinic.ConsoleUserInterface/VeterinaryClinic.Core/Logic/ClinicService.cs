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

        /// <summary>
        /// Инициализирует новый экземпляр сервиса клиники с указанными репозиториями.
        /// </summary>
        /// <param name="ownerRepo">Репозиторий владельцев</param>
        /// <param name="petRepo">Репозиторий животных</param>
        /// <param name="vetRepo">Репозиторий ветеринаров</param>
        /// <param name="appointmentRepo">Репозиторий записей на прием</param>
        /// <param name="visitHistoryRepo">Репозиторий истории посещений</param>
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

        /// <summary>
        /// Создает нового владельца.
        /// </summary>
        /// <param name="fullName">Полное имя владельца</param>
        /// <param name="phoneNumber">Номер телефона владельца</param>
        /// <returns>Созданный владелец</returns>
        public Owner CreateOwner(string fullName, string phoneNumber)
        {
            var owner = new Owner(Guid.NewGuid(), fullName, phoneNumber);
            var ownerDto = OwnerMapper.ToDto(owner);
            _ownerRepo.Add(ownerDto);
            _ownerRepo.Save();
            return owner;
        }

        /// <summary>
        /// Находит владельцев по имени (поиск без учета регистра).
        /// </summary>
        /// <param name="name">Имя для поиска</param>
        /// <returns>Список найденных владельцев</returns>
        public List<Owner> FindOwnersByName(string name)
        {
            var allOwners = _ownerRepo.GetAll();
            var filteredOwners = allOwners.Where(o =>
                !string.IsNullOrEmpty(o.FullName) &&
                o.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
            return filteredOwners.Select(OwnerMapper.ToDomain).ToList();
        }

        /// <summary>
        /// Получает всех владельцев.
        /// </summary>
        /// <returns>Список всех владельцев</returns>
        public List<Owner> GetAllOwners()
        {
            var allOwners = _ownerRepo.GetAll();
            return allOwners.Select(OwnerMapper.ToDomain).ToList();
        }

        /// <summary>
        /// Обновляет данные владельца.
        /// </summary>
        /// <param name="owner">Объект владельца для обновления</param>
        public void UpdateOwner(Owner owner)
        {
            var dto = OwnerMapper.ToDto(owner);
            _ownerRepo.Update(dto);
            _ownerRepo.Save();
        }

        /// <summary>
        /// Удаляет владельца по идентификатору.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца</param>
        /// <returns>True если удаление успешно, иначе False</returns>
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

        /// <summary>
        /// Создает новое животное.
        /// </summary>
        /// <param name="name">Имя животного</param>
        /// <param name="species">Вид животного</param>
        /// <param name="breed">Порода животного</param>
        /// <param name="ownerId">Идентификатор владельца</param>
        /// <returns>Созданное животное</returns>
        public Pet CreatePet(string name, string species, string breed, Guid ownerId)
        {
            var pet = new Pet(Guid.NewGuid(), name, species, breed, ownerId);
            var dto = PetMapper.ToDto(pet);
            _petRepo.Add(dto);
            _petRepo.Save();
            return pet;
        }

        /// <summary>
        /// Получает всех животных указанного владельца.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца</param>
        /// <returns>Список животных владельца</returns>
        public List<Pet> GetPetsByOwnerId(Guid ownerId)
        {
            var pets = _petRepo.GetAll().Where(p => p.OwnerId == ownerId);
            return pets.Select(PetMapper.ToDomain).ToList();
        }

        /// <summary>
        /// Получает всех животных.
        /// </summary>
        /// <returns>Список всех животных</returns>
        public List<Pet> GetAllPets()
        {
            return _petRepo.GetAll()
                .Select(PetMapper.ToDomain)
                .ToList();
        }

        /// <summary>
        /// Обновляет данные животного.
        /// </summary>
        /// <param name="pet">Объект животного для обновления</param>
        public void UpdatePet(Pet pet)
        {
            var dto = PetMapper.ToDto(pet);
            _petRepo.Update(dto);
            _petRepo.Save();
        }

        /// <summary>
        /// Удаляет животное по идентификатору.
        /// </summary>
        /// <param name="petId">Идентификатор животного</param>
        /// <returns>True если удаление успешно, иначе False</returns>
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

        /// <summary>
        /// Получает ветеринара, работающего в указанный день недели.
        /// </summary>
        /// <param name="day">День недели</param>
        /// <returns>Ветеринар или null если не найден</returns>
        public Veterinarian GetVeterinarianByDay(DayOfWeek day)
        {
            var allVets = _vetRepo.GetAll();
            var vetDto = allVets.FirstOrDefault(v =>
                !string.IsNullOrEmpty(v.WorkDaysString) &&
                v.WorkDaysString.Split(',').Contains(day.ToString()));
            return vetDto == null ? null : VeterinarianMapper.ToDomain(vetDto);
        }

        /// <summary>
        /// Получает всех ветеринаров.
        /// </summary>
        /// <returns>Список всех ветеринаров</returns>
        public List<Veterinarian> GetAllVeterinarians()
        {
            return _vetRepo.GetAll()
                .Select(VeterinarianMapper.ToDomain)
                .ToList();
        }

        // === Комплексные бизнес-методы ===

        /// <summary>
        /// Удаляет владельца и всех его животных.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца</param>
        /// <returns>True если удаление успешно, иначе False</returns>
        public bool DeleteOwnerWithPets(Guid ownerId)
        {
            var pets = GetPetsByOwnerId(ownerId);
            foreach (var pet in pets)
                DeletePet(pet.Id);

            return DeleteOwner(ownerId);
        }

        /// <summary>
        /// Получает доступные временные слоты для ветеринара на указанную дату.
        /// </summary>
        /// <param name="vetId">Идентификатор ветеринара</param>
        /// <param name="date">Дата для проверки</param>
        /// <returns>Список доступных временных слотов</returns>
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

        /// <summary>
        /// Проверяет доступность временного слота для ветеринара.
        /// </summary>
        /// <param name="vetId">Идентификатор ветеринара</param>
        /// <param name="slot">Временной слот для проверки</param>
        /// <returns>True если слот доступен, иначе False</returns>
        private bool IsTimeSlotAvailable(Guid vetId, DateTime slot)
        {
            var appointments = _appointmentRepo.GetAll()
                .Where(a => a.VeterinarianId == vetId &&
                           a.AppointmentDate.Date == slot.Date &&
                           a.TimeSlot == slot.ToString("HH:mm"));
            return !appointments.Any();
        }

        /// <summary>
        /// Создает запись на прием и добавляет запись в историю посещений.
        /// </summary>
        /// <param name="petId">Идентификатор животного</param>
        /// <param name="vetId">Идентификатор ветеринара</param>
        /// <param name="appointmentDate">Дата приема</param>
        /// <param name="timeSlot">Временной слот</param>
        /// <param name="reason">Причина визита</param>
        /// <param name="breed">Порода животного</param>
        /// <param name="diagnosis">Диагноз</param>
        /// <param name="treatment">Лечение</param>
        /// <param name="notes">Примечания</param>
        /// <returns>Созданная запись на прием</returns>
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

        /// <summary>
        /// Получает животное по идентификатору.
        /// </summary>
        /// <param name="petId">Идентификатор животного</param>
        /// <returns>Животное или null если не найдено</returns>
        public Pet GetPet(Guid petId)
        {
            var petDto = _petRepo.GetById(petId);
            return petDto == null ? null : PetMapper.ToDomain(petDto);
        }

        /// <summary>
        /// Получает ветеринара по идентификатору.
        /// </summary>
        /// <param name="vetId">Идентификатор ветеринара</param>
        /// <returns>Ветеринар или null если не найден</returns>
        public Veterinarian GetVeterinarian(Guid vetId)
        {
            var vetDto = _vetRepo.GetById(vetId);
            return vetDto == null ? null : VeterinarianMapper.ToDomain(vetDto);
        }

        /// <summary>
        /// Получает все записи на прием на указанную дату.
        /// </summary>
        /// <param name="date">Дата для поиска</param>
        /// <returns>Список записей на указанную дату</returns>
        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return _appointmentRepo.GetAll()
                .Where(a => a.AppointmentDate.Date == date.Date)
                .Select(AppointmentMapper.ToDomain)
                .ToList();
        }

        // === Методы для работы с расписанием и историей ===

        /// <summary>
        /// Получает данные расписания на указанную дату.
        /// </summary>
        /// <param name="date">Дата для получения расписания</param>
        /// <returns>Данные расписания</returns>
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

        /// <summary>
        /// Получает историю посещений для владельца по имени.
        /// </summary>
        /// <param name="ownerName">Имя владельца</param>
        /// <returns>Список истории посещений, отсортированный по дате (сначала новые)</returns>
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

        /// <summary>
        /// Удаляет запись на прием по идентификатору.
        /// </summary>
        /// <param name="appointmentId">Идентификатор записи на прием</param>
        /// <returns>True если удаление успешно, иначе False</returns>
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

    /// <summary>
    /// Представляет данные расписания на определенную дату.
    /// </summary>
    public class ScheduleData
    {
        /// <summary>
        /// Список записей на прием.
        /// </summary>
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        /// <summary>
        /// Список ветеринаров.
        /// </summary>
        public List<Veterinarian> Veterinarians { get; set; } = new List<Veterinarian>();
    }
}