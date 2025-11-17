using DataAccessL;
using DataAccessL.dapper;
using DataAccessL.Ef;
using Dto.Essence;
using Dto.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.Essence;
using VeterinaryClinic.Core.Logic;
using Microsoft.SqlServer;
using System.Data.SqlClient;

namespace VeterinaryClinic.ConsoleUserInterface
{
    class Program
    {
        private static ClinicService _clinicService;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ВЕТЕРИНАРНАЯ КЛИНИКА ===");
            Console.WriteLine("Выберите технологию работы с данными:");
            Console.WriteLine("1. Entity Framework");
            Console.WriteLine("2. Dapper");
            Console.Write("Ваш выбор (1-2): ");

            var choice = Console.ReadLine();
            bool useDapper = choice == "2";

            ConfigureServices(useDapper);

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== ВЕТЕРИНАРНАЯ КЛИНИКА ===");
                Console.WriteLine($"Режим: {(useDapper ? "Dapper" : "Entity Framework")}");
                Console.WriteLine("1. Управление владельцами");
                Console.WriteLine("2. Управление питомцами");
                Console.WriteLine("3. Запись на прием");
                Console.WriteLine("4. История визитов питомца");
                Console.WriteLine("5. Просмотр расписания");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите действие: ");

                string menuChoice = Console.ReadLine();

                try
                {
                    switch (menuChoice)
                    {
                        case "1":
                            OwnerMenu();
                            break;
                        case "2":
                            PetMenu();
                            break;
                        case "3":
                            CreateAppointment();
                            break;
                        case "4":
                            ShowVisitHistory();
                            break;
                        case "5":
                            ShowSchedule();
                            break;
                        case "6":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор!");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void ConfigureServices(bool useDapper)
        {
            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\настя\\source\\repos\\VetClinic_Repo\\VtCl\\VeterinaryClinic.ConsoleUserInterface\\VeterinaryClinic.ConsoleUserInterface\\Database_VetCl.mdf;Integrated Security=True";

            if (useDapper)
            {
                // Для Dapper сначала создаем базу через EF
                CreateDatabaseWithEF(connectionString);

                // Затем настраиваем Dapper репозитории
                IBaseRepository<OwnerDto> ownerRepo = new DapperOwnerRepo(connectionString);
                IBaseRepository<PetDto> petRepo = new DapperPetRepo(connectionString);
                IBaseRepository<VeterinarianDto> vetRepo = new DapperVeterinarianRepo(connectionString);
                IBaseRepository<AppointmentDto> appointmentRepo = new DapperAppointmentRepo(connectionString);
                IBaseRepository<VisitHistoryDto> visitHistoryRepo = new DapperVisitHistoryRepo(connectionString);

                _clinicService = new ClinicService(ownerRepo, petRepo, vetRepo, appointmentRepo, visitHistoryRepo);
            }
            else
            {
                // Настройка EF репозиториев
                var options = new DbContextOptionsBuilder<Context>()
                    .UseSqlServer(connectionString)
                    .Options;

                var context = new Context(options);
                CreateDatabaseWithEF(connectionString);

                IBaseRepository<OwnerDto> ownerRepo = new EfOwnerRepo(context);
                IBaseRepository<PetDto> petRepo = new EfPetRepo(context);
                IBaseRepository<VeterinarianDto> vetRepo = new EfVeterinarianRepo(context);
                IBaseRepository<AppointmentDto> appointmentRepo = new EfAppointmentRepo(context);
                IBaseRepository<VisitHistoryDto> visitHistoryRepo = new EfVisitHistoryRepo(context);

                _clinicService = new ClinicService(ownerRepo, petRepo, vetRepo, appointmentRepo, visitHistoryRepo);
            }
        }

        static void CreateDatabaseWithEF(string connectionString)
        {
            try
            {
                var options = new DbContextOptionsBuilder<Context>()
                    .UseSqlServer(connectionString)
                    .Options;

                using (var context = new Context(options))
                {
                    Console.WriteLine("🔄 Создание/проверка базы данных...");

                    // Просто создаем базу
                    context.Database.EnsureCreated();

                    // Добавляем ветеринаров если их нет
                    if (!context.Veterinarians.Any())
                    {
                        InitializeVeterinarians(context);
                    }

                    Console.WriteLine("✅ База данных готова к работе!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при создании базы: {ex.Message}");
                throw;
            }
        }

        // Метод для инициализации ветеринаров
        static void InitializeVeterinarians(Context context)
        {
            if (context.Veterinarians.Any())
            {
                Console.WriteLine("✅ Ветеринары уже есть в базе.");
                return;
            }

            var veterinarians = new[]
            {
        new { Name = "Иванов И.И.", Days = new[] { DayOfWeek.Monday, DayOfWeek.Thursday } },
        new { Name = "Петров П.П.", Days = new[] { DayOfWeek.Tuesday, DayOfWeek.Friday } },
        new { Name = "Сидоров С.С.", Days = new[] { DayOfWeek.Wednesday, DayOfWeek.Saturday } }
    };

            foreach (var vet in veterinarians)
            {
                var vetDto = new VeterinarianDto
                {
                    Id = Guid.NewGuid(),
                    FullName = vet.Name,
                    WorkDaysString = string.Join(",", vet.Days.Select(d => d.ToString()))
                };
                context.Veterinarians.Add(vetDto);
            }

            context.SaveChanges();
            Console.WriteLine($"✅ Добавлено {veterinarians.Length} ветеринаров в базу.");
        }


        static void OwnerMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ ВЛАДЕЛЬЦАМИ ===");
                Console.WriteLine("1. Добавить владельца");
                Console.WriteLine("2. Показать всех владельцев");
                Console.WriteLine("3. Найти владельца по имени");
                Console.WriteLine("4. Удалить владельца");
                Console.WriteLine("5. Назад");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddOwner();
                            break;
                        case "2":
                            ShowAllOwners();
                            break;
                        case "3":
                            FindOwnerByName();
                            break;
                        case "4":
                            DeleteOwner();
                            break;
                        case "5":
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }

                if (choice != "5")
                {
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void AddOwner()
        {
            Console.Write("Введите ФИО: ");
            string fullName = Console.ReadLine();
            Console.Write("Введите телефон: ");
            string phone = Console.ReadLine();

            var owner = _clinicService.CreateOwner(fullName, phone);
            Console.WriteLine($"✅ Владелец добавлен: {owner}");
        }

        static void ShowAllOwners()
        {
            var owners = _clinicService.GetAllOwners();
            if (owners.Count == 0)
            {
                Console.WriteLine("📭 Владельцев нет.");
                return;
            }

            Console.WriteLine("\n📋 Список всех владельцев:");
            foreach (var owner in owners)
            {
                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} | 📞 {owner.PhoneNumber}");
            }
        }

        static void FindOwnerByName()
        {
            Console.Write("Введите имя для поиска: ");
            string name = Console.ReadLine();

            var found = _clinicService.FindOwnersByName(name);
            if (found.Count == 0)
            {
                Console.WriteLine("❌ Владельцы не найдены.");
                return;
            }

            Console.WriteLine("\n🔍 Найденные владельцы:");
            foreach (var owner in found)
            {
                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} | 📞 {owner.PhoneNumber}");
            }
        }

        static void DeleteOwner()
        {
            Console.Write("Введите фамилию владельца для поиска: ");
            string searchLastName = Console.ReadLine();

            var foundOwners = _clinicService.FindOwnersByName(searchLastName);
            if (foundOwners.Count == 0)
            {
                Console.WriteLine("❌ Владельцы с такой фамилией не найдены!");
                return;
            }

            Console.WriteLine("\n🔍 Найденные владельцы:");
            foreach (var owner in foundOwners)
            {
                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} ({owner.PhoneNumber})");
            }

            Console.Write("Введите ID владельца для удаления: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid deleteOwnerId))
            {
                Console.WriteLine("❌ Неверный формат ID!");
                return;
            }

            var selectedOwner = foundOwners.FirstOrDefault(o => o.Id == deleteOwnerId);
            if (selectedOwner == null)
            {
                Console.WriteLine("❌ Выбранный ID не соответствует найденным владельцам!");
                return;
            }

            var pets = _clinicService.GetPetsByOwnerId(deleteOwnerId);
            if (pets.Count > 0)
            {
                Console.WriteLine($"\n⚠️ У владельца {selectedOwner.FullName} есть питомцы:");
                foreach (var pet in pets)
                {
                    Console.WriteLine($"  - {pet.Name} ({pet.Species} - {pet.Breed})");
                }
                Console.Write("При удалении владельца будут удалены все его питомцы. Продолжить? (да/нет): ");
            }
            else
            {
                Console.Write($"Вы уверены, что хотите удалить владельца {selectedOwner.FullName}? (да/нет): ");
            }

            string confirmation = Console.ReadLine()?.ToLower();
            if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
            {
                if (_clinicService.DeleteOwnerWithPets(deleteOwnerId))
                {
                    Console.WriteLine("✅ Владелец и питомцы успешно удалены.");
                }
                else
                {
                    Console.WriteLine("❌ Ошибка при удалении владельца.");
                }
            }
            else
            {
                Console.WriteLine("❌ Удаление отменено.");
            }
        }

        static void PetMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== УПРАВЛЕНИЕ ПИТОМЦАМИ ===");
                Console.WriteLine("1. Добавить питомца");
                Console.WriteLine("2. Показать всех питомцев");
                Console.WriteLine("3. Найти питомцев по владельцу");
                Console.WriteLine("4. Удалить питомца");
                Console.WriteLine("5. Назад");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddPet();
                            break;
                        case "2":
                            ShowAllPets();
                            break;
                        case "3":
                            FindPetsByOwner();
                            break;
                        case "4":
                            DeletePet();
                            break;
                        case "5":
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }

                if (choice != "5")
                {
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void AddPet()
        {
            Console.Write("Введите фамилию владельца для поиска: ");
            string searchLastName = Console.ReadLine();

            var foundOwners = _clinicService.FindOwnersByName(searchLastName);
            if (foundOwners.Count == 0)
            {
                Console.WriteLine("❌ Владельцы с такой фамилией не найдены!");
                return;
            }

            Console.WriteLine("\n🔍 Найденные владельцы:");
            foreach (var owner in foundOwners)
            {
                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} ({owner.PhoneNumber})");
            }

            Console.Write("Введите ID владельца: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid ownerId))
            {
                Console.WriteLine("❌ Неверный формат ID!");
                return;
            }

            var selectedOwner = foundOwners.FirstOrDefault(o => o.Id == ownerId);
            if (selectedOwner == null)
            {
                Console.WriteLine("❌ Выбранный ID не соответствует найденным владельцам!");
                return;
            }

            Console.Write("Введите кличку питомца: ");
            string petName = Console.ReadLine();
            Console.Write("Введите вид питомца: ");
            string species = Console.ReadLine();
            Console.Write("Введите породу питомца: ");
            string breed = Console.ReadLine();

            try
            {
                var pet = _clinicService.CreatePet(petName, species, breed, ownerId);
                Console.WriteLine($"✅ Питомец добавлен: {pet}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Ошибка при добавлении питомца: " + ex.Message);
            }
        }

        static void ShowAllPets()
        {
            var pets = _clinicService.GetAllPets();
            if (pets.Count == 0)
            {
                Console.WriteLine("📭 Питомцев нет.");
                return;
            }

            Console.WriteLine("\n📋 Список всех питомцев:");
            foreach (var pet in pets)
            {
                var owner = _clinicService.FindOwnersByName("").Find(o => o.Id == pet.OwnerId);
                Console.WriteLine($"ID: {pet.Id} | {pet.Name} ({pet.Species} - {pet.Breed}) | Владелец: {owner?.FullName ?? "Неизвестен"}");
            }
        }

        static void FindPetsByOwner()
        {
            Console.Write("Введите фамилию владельца для поиска: ");
            string searchOwnerName = Console.ReadLine();

            var foundOwners = _clinicService.FindOwnersByName(searchOwnerName);
            if (foundOwners.Count == 0)
            {
                Console.WriteLine("❌ Владельцы с такой фамилией не найдены!");
                return;
            }

            Console.WriteLine("\n🔍 Найденные владельцы:");
            foreach (var owner in foundOwners)
            {
                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} ({owner.PhoneNumber})");
            }

            Console.Write("Введите ID владельца: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid ownerId))
            {
                Console.WriteLine("❌ Неверный формат ID!");
                return;
            }

            var selectedOwner = foundOwners.Find(o => o.Id == ownerId);
            if (selectedOwner == null)
            {
                Console.WriteLine("❌ Выбранный ID не соответствует найденным владельцам!");
                return;
            }

            var pets = _clinicService.GetPetsByOwnerId(ownerId);
            if (pets.Count == 0)
            {
                Console.WriteLine($"📭 У владельца {selectedOwner.FullName} нет питомцев!");
                return;
            }

            Console.WriteLine($"\n🐾 Питомцы владельца {selectedOwner.FullName}:");
            foreach (var pet in pets)
            {
                Console.WriteLine($"ID: {pet.Id} | {pet.Name} ({pet.Species} - {pet.Breed})");
            }
        }

        static void DeletePet()
        {
            Console.Write("Введите фамилию владельца для поиска: ");
            string searchOwnerName = Console.ReadLine();

            var foundOwners = _clinicService.FindOwnersByName(searchOwnerName);
            if (foundOwners.Count == 0)
            {
                Console.WriteLine("❌ Владельцы с такой фамилией не найдены!");
                return;
            }

            Console.WriteLine("\n🔍 Найденные владельцы:");
            foreach (var owner in foundOwners)
            {
                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} ({owner.PhoneNumber})");
            }

            Console.Write("Введите ID владельца: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid ownerId))
            {
                Console.WriteLine("❌ Неверный формат ID!");
                return;
            }

            var selectedOwner = foundOwners.Find(o => o.Id == ownerId);
            if (selectedOwner == null)
            {
                Console.WriteLine("❌ Выбранный ID не соответствует найденным владельцам!");
                return;
            }

            var pets = _clinicService.GetPetsByOwnerId(ownerId);
            if (pets.Count == 0)
            {
                Console.WriteLine($"📭 У владельца {selectedOwner.FullName} нет питомцев для удаления!");
                return;
            }

            Console.WriteLine($"\n🐾 Питомцы владельца {selectedOwner.FullName}:");
            for (int i = 0; i < pets.Count; i++)
            {
                var pet = pets[i];
                Console.WriteLine($"{i + 1}. ID: {pet.Id} | {pet.Name} ({pet.Species} - {pet.Breed})");
            }

            Console.Write("Выберите номер питомца для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int petIndex) || petIndex < 1 || petIndex > pets.Count)
            {
                Console.WriteLine("❌ Неверный выбор!");
                return;
            }

            var petToDelete = pets[petIndex - 1];
            Console.Write($"Вы уверены, что хотите удалить питомца {petToDelete.Name}? (да/нет): ");

            string confirmation = Console.ReadLine()?.ToLower();
            if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
            {
                bool deleted = _clinicService.DeletePet(petToDelete.Id);
                Console.WriteLine(deleted ? "✅ Питомец успешно удален." : "❌ Ошибка при удалении питомца.");
            }
            else
            {
                Console.WriteLine("❌ Удаление отменено.");
            }
        }

        static void CreateAppointment()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАПИСЬ НА ПРИЕМ ===");

            Console.Write("Введите ФИО владельца: ");
            string ownerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                Console.WriteLine("❌ ФИО не может быть пустым.");
                Console.ReadKey();
                return;
            }

            var owners = _clinicService.FindOwnersByName(ownerName);
            if (owners.Count == 0)
            {
                Console.WriteLine("❌ Владельцы не найдены.");
                Console.ReadKey();
                return;
            }

            Owner selectedOwner;
            if (owners.Count == 1)
            {
                selectedOwner = owners[0];
            }
            else
            {
                Console.WriteLine("🔍 Найдено несколько владельцев:");
                for (int i = 0; i < owners.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {owners[i].FullName} ({owners[i].PhoneNumber})");
                }
                Console.Write("Выберите номер владельца: ");
                if (!int.TryParse(Console.ReadLine(), out int ownerIndex) || ownerIndex < 1 || ownerIndex > owners.Count)
                {
                    Console.WriteLine("❌ Неверный выбор.");
                    Console.ReadKey();
                    return;
                }
                selectedOwner = owners[ownerIndex - 1];
            }

            var pets = _clinicService.GetPetsByOwnerId(selectedOwner.Id);
            if (pets.Count == 0)
            {
                Console.WriteLine("❌ У владельца нет питомцев.");
                Console.ReadKey();
                return;
            }

            Pet selectedPet;
            if (pets.Count == 1)
            {
                selectedPet = pets[0];
            }
            else
            {
                Console.WriteLine("🐾 Выберите питомца:");
                for (int i = 0; i < pets.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {pets[i].Name} ({pets[i].Species} - {pets[i].Breed})");
                }
                if (!int.TryParse(Console.ReadLine(), out int petIndex) || petIndex < 1 || petIndex > pets.Count)
                {
                    Console.WriteLine("❌ Неверный выбор.");
                    Console.ReadKey();
                    return;
                }
                selectedPet = pets[petIndex - 1];
            }

            Console.Write("Введите дату (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("❌ Неверный формат даты.");
                Console.ReadKey();
                return;
            }

            if (date.Date < DateTime.Today)
            {
                Console.WriteLine("❌ Нельзя записаться на прошедшую дату.");
                Console.ReadKey();
                return;
            }

            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine("❌ Воскресенье - выходной.");
                Console.ReadKey();
                return;
            }

            var vet = _clinicService.GetVeterinarianByDay(date.DayOfWeek);
            if (vet == null)
            {
                Console.WriteLine("❌ Нет врачей в этот день.");
                Console.ReadKey();
                return;
            }

            var slots = _clinicService.GetAvailableTimeSlots(vet.Id, date);
            if (slots.Count == 0)
            {
                Console.WriteLine("❌ Нет свободных временных слотов.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("⏰ Выберите время:");
            for (int i = 0; i < slots.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {slots[i]:HH:mm}");
            }

            if (!int.TryParse(Console.ReadLine(), out int slotIndex) || slotIndex < 1 || slotIndex > slots.Count)
            {
                Console.WriteLine("❌ Неверный выбор.");
                Console.ReadKey();
                return;
            }

            var selectedTime = slots[slotIndex - 1];
            Console.Write("Причина визита (по умолчанию: Плановый осмотр): ");
            string reason = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(reason)) reason = "Плановый осмотр";

            try
            {
                var appointment = _clinicService.CreateAppointment(
                    selectedPet.Id,
                    vet.Id,
                    selectedTime,
                    selectedTime.ToString("HH:mm"),
                    reason,
                    selectedPet.Breed,
                    diagnosis: "",
                    treatment: "",
                    notes: "");

                Console.WriteLine("✅ Запись успешно создана:");
                Console.WriteLine($"👤 Владелец: {selectedOwner.FullName}");
                Console.WriteLine($"🐾 Питомец: {selectedPet.Name}");
                Console.WriteLine($"👨‍⚕️ Врач: {vet.FullName}");
                Console.WriteLine($"📅 Дата и время: {selectedTime:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"📝 Причина: {reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Ошибка при создании записи: " + ex.Message);
            }
            Console.ReadKey();
        }

        static void ShowVisitHistory()
        {
            Console.Clear();
            Console.WriteLine("=== ИСТОРИЯ ВИЗИТОВ ПИТОМЦА ===");

            Console.Write("Введите ФИО владельца: ");
            string ownerName = Console.ReadLine();

            try
            {
                var history = _clinicService.GetVisitHistoryByOwner(ownerName);
                var owner = _clinicService.FindOwnersByName(ownerName).First();
                var pets = _clinicService.GetPetsByOwnerId(owner.Id);
                var pet = pets.First();

                Console.Clear();
                Console.WriteLine($"📋 История визитов питомца {pet.Name}:\n");

                if (history.Count == 0)
                {
                    Console.WriteLine("📭 История визитов пуста.");
                }
                else
                {
                    foreach (var visit in history)
                    {
                        Console.WriteLine($"📅 Дата: {visit.VisitDate:dd.MM.yyyy HH:mm}");
                        Console.WriteLine($"👨‍⚕️ Врач: {visit.VeterinarianName}");
                        Console.WriteLine($"📝 Причина: {visit.Reason}");
                        if (!string.IsNullOrEmpty(visit.Diagnosis))
                            Console.WriteLine($"🩺 Диагноз: {visit.Diagnosis}");
                        if (!string.IsNullOrEmpty(visit.Treatment))
                            Console.WriteLine($"💊 Лечение: {visit.Treatment}");
                        if (!string.IsNullOrEmpty(visit.Notes))
                            Console.WriteLine($"📄 Заметки: {visit.Notes}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Ошибка: " + ex.Message);
            }

            Console.WriteLine("Нажмите любую клавишу для возврата...");
            Console.ReadKey();
        }

        static void ShowSchedule()
        {
            Console.Clear();
            Console.WriteLine("=== РАСПИСАНИЕ ВРАЧЕЙ ===");

            Console.Write("Введите дату (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("❌ Неверный формат даты.");
                Console.ReadKey();
                return;
            }

            try
            {
                var scheduleData = _clinicService.GetScheduleData(date);
                var appointments = scheduleData.Appointments;
                var veterinarians = scheduleData.Veterinarians;

                Console.WriteLine($"\n📅 Расписание на {date:dd.MM.yyyy}:\n");

                Console.Write("Время     ");
                foreach (var vet in veterinarians)
                {
                    if (vet.WorkDays.Contains(date.DayOfWeek))
                        Console.Write($"| {vet.FullName,-12} ");
                }
                Console.WriteLine();

                Console.Write(new string('-', 10));
                foreach (var vet in veterinarians)
                {
                    if (vet.WorkDays.Contains(date.DayOfWeek))
                        Console.Write($"+{new string('-', 14)}");
                }
                Console.WriteLine();

                for (int hour = 10; hour < 19; hour++)
                {
                    var timeSlot = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                    Console.Write($"{timeSlot:HH:mm}     ");

                    foreach (var vet in veterinarians)
                    {
                        if (vet.WorkDays.Contains(date.DayOfWeek))
                        {
                            var appointment = appointments.Find(a => a.VeterinarianId == vet.Id && a.AppointmentDate == timeSlot);
                            if (appointment != null)
                            {
                                var petName = _clinicService.GetPet(appointment.PetId)?.Name ?? "Неизвестен";
                                Console.Write($"| {petName} ({appointment.Reason})  ");
                            }
                            else
                            {
                                Console.Write("| [СВОБОДНО]    ");
                            }
                        }
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Ошибка: " + ex.Message);
            }

            Console.WriteLine("Нажмите любую клавишу для возврата...");
            Console.ReadKey();
        }

    }
}