using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.Logic;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.ConsoleUserInterface
{
    internal class Program
    {
        static ClinicService clinicService;

        static void Main(string[] args)
        {
            var ownerManager = new OwnerManager();
            var petManager = new PetManager(ownerManager);
            var vetManager = new VeterinarianManager();

            clinicService = new ClinicService(ownerManager, petManager, vetManager);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ВЕТЕРИНАРНАЯ КЛИНИКА ===");
                Console.WriteLine("1. Управление владельцами");
                Console.WriteLine("2. Управление питомцами");
                Console.WriteLine("3. Запись на прием");
                Console.WriteLine("4. История визитов питомца");
                Console.WriteLine("5. Просмотр расписания");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
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
                            return;
                        default:
                            Console.WriteLine("Неверный выбор! Нажмите любую клавишу...");
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

        static void OwnerMenu()
        {
            while (true)
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
                            Console.Write("Введите ФИО: ");
                            string fullName = Console.ReadLine();
                            Console.Write("Введите телефон: ");
                            string phone = Console.ReadLine();
                            var owner = clinicService.CreateOwner(fullName, phone);
                            Console.WriteLine($"Владелец добавлен: {owner}");
                            break;

                        case "2":
                            var owners = clinicService.GetAllOwners();
                            if (owners.Count == 0)
                                Console.WriteLine("Владельцев нет.");
                            else
                                owners.ForEach(o => Console.WriteLine(o));
                            break;

                        case "3":
                            Console.Write("Введите имя для поиска: ");
                            string name = Console.ReadLine();
                            var found = clinicService.FindOwnersByName(name);
                            if (found.Count == 0)
                                Console.WriteLine("Владельцы не найдены.");
                            else
                                found.ForEach(o => Console.WriteLine(o));
                            break;

                        case "4":
                            Console.Write("Введите фамилию владельца для поиска: ");
                            string searchLastNameForDelete = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(searchLastNameForDelete))
                            {
                                Console.WriteLine("Фамилия не может быть пустой!");
                                break;
                            }

                            List<Owner> foundOwnersForDelete = clinicService.FindOwnersByName(searchLastNameForDelete);

                            if (foundOwnersForDelete.Count == 0)
                            {
                                Console.WriteLine("Владельцы с такой фамилией не найдены!");
                                break;
                            }

                            Console.WriteLine("\nНайденные владельцы:");
                            foreach (Owner currentOwner in foundOwnersForDelete)
                            {
                                Console.WriteLine($"ID: {currentOwner.Id} | {currentOwner.FullName} ({currentOwner.PhoneNumber})");
                            }

                            Console.Write("Введите ID владельца для удаления: ");
                            if (!int.TryParse(Console.ReadLine(), out int deleteOwnerId))
                            {
                                Console.WriteLine("Неверный формат ID!");
                                break;
                            }

                            var selectedOwnerForDelete = foundOwnersForDelete.FirstOrDefault(o => o.Id == deleteOwnerId);

                            if (selectedOwnerForDelete == null)
                            {
                                Console.WriteLine("Выбранный ID не соответствует найденным владельцам!");
                                break;
                            }

                            var petsToDelete = clinicService.GetPetsByOwnerId(deleteOwnerId);
                            if (petsToDelete.Count > 0)
                            {
                                Console.WriteLine($"\nУ владельца {selectedOwnerForDelete.FullName} есть питомцы:");
                                foreach (var pet in petsToDelete)
                                {
                                    Console.WriteLine($"  - {pet.Name} ({pet.Species} - {pet.Breed})");
                                }
                                Console.Write("При удалении владельца будут удалены все его питомцы. Продолжить? (да/нет): ");
                            }
                            else
                            {
                                Console.Write($"Вы уверены, что хотите удалить владельца {selectedOwnerForDelete.FullName}? (да/нет): ");
                            }

                            string confirmation = Console.ReadLine()?.ToLower();

                            if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
                            {
                                if (clinicService.DeleteOwnerWithPets(deleteOwnerId))
                                {
                                    Console.WriteLine("Владелец и питомцы успешно удалены.");
                                }
                                else
                                {
                                    Console.WriteLine("Ошибка при удалении владельца.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Удаление отменено.");
                            }
                            break;

                        case "5":
                            return;

                        default:
                            Console.WriteLine("Неверный выбор.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void PetMenu()
        {
            while (true)
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
                            Console.Write("Введите фамилию владельца для поиска: ");
                            string searchLastName = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(searchLastName))
                            {
                                Console.WriteLine("Фамилия не может быть пустой!");
                                break;
                            }

                            List<Owner> foundOwners = clinicService.FindOwnersByName(searchLastName);

                            if (foundOwners.Count == 0)
                            {
                                Console.WriteLine("Владельцы с такой фамилией не найдены!");
                                break;
                            }

                            Console.WriteLine("\nНайденные владельцы:");
                            foreach (Owner owner in foundOwners)
                            {
                                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} ({owner.PhoneNumber})");
                            }

                            Console.Write("Введите ID владельца: ");
                            if (!int.TryParse(Console.ReadLine(), out int ownerId))
                            {
                                Console.WriteLine("Неверный формат ID!");
                                break;
                            }

                            var selectedOwner = foundOwners.FirstOrDefault(o => o.Id == ownerId);
                            if (selectedOwner == null)
                            {
                                Console.WriteLine("Выбранный ID не соответствует найденным владельцам!");
                                break;
                            }

                            Console.Write("Введите кличку питомца: ");
                            string petName = Console.ReadLine();

                            Console.Write("Введите вид питомца: ");
                            string species = Console.ReadLine();

                            Console.Write("Введите породу питомца: ");
                            string breed = Console.ReadLine();

                            try
                            {
                                Pet pet = clinicService.CreatePet(petName, species, breed, ownerId);
                                Console.WriteLine($"Питомец добавлен: {pet}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ошибка при добавлении питомца: " + ex.Message);
                            }
                            break;


                        case "2":
                            var pets = clinicService.GetAllPets();
                            if (pets.Count == 0)
                                Console.WriteLine("Питомцев нет.");
                            else
                            {
                                foreach (var p in pets)
                                {
                                    var owner = clinicService.FindOwnersByName("").Find(o => o.Id == p.OwnerId);
                                    Console.WriteLine($"{p} | Владелец: {(owner != null ? owner.FullName : "Неизвестен")}");
                                }
                            }
                            break;

                        case "3":
                            Console.Write("Введите фамилию владельца для поиска: ");
                            string searchOwnerName = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(searchOwnerName))
                            {
                                Console.WriteLine("Фамилия не может быть пустой!");
                                break;
                            }

                            List<Owner> foundOwnersForPets = clinicService.FindOwnersByName(searchOwnerName);

                            if (foundOwnersForPets.Count == 0)
                            {
                                Console.WriteLine("Владельцы с такой фамилией не найдены!");
                                break;
                            }

                            Console.WriteLine("\nНайденные владельцы:");
                            foreach (Owner owner in foundOwnersForPets)
                            {
                                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} ({owner.PhoneNumber})");
                            }

                            Console.Write("Введите ID владельца: ");
                            if (!int.TryParse(Console.ReadLine(), out int ownerIdSearch))
                            {
                                Console.WriteLine("Неверный формат ID!");
                                break;
                            }

                            var selectedOwnerForPets = foundOwnersForPets.Find(o => o.Id == ownerIdSearch);
                            if (selectedOwnerForPets == null)
                            {
                                Console.WriteLine("Выбранный ID не соответствует найденным владельцам!");
                                break;
                            }

                            List<Pet> petsByOwner = clinicService.GetPetsByOwnerId(ownerIdSearch);
                            if (petsByOwner.Count == 0)
                            {
                                Console.WriteLine($"У владельца {selectedOwnerForPets.FullName} нет питомцев!");
                            }
                            else
                            {
                                Console.WriteLine($"\nПитомцы владельца {selectedOwnerForPets.FullName}:");
                                foreach (Pet currentPet in petsByOwner)
                                {
                                    Console.WriteLine($"{currentPet}");
                                }
                            }
                            break;


                        case "4":
                            Console.Write("Введите фамилию владельца для поиска: ");
                            string searchOwnerForDelete = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(searchOwnerForDelete))
                            {
                                Console.WriteLine("Фамилия не может быть пустой!");
                                break;
                            }

                            List<Owner> foundOwnersForDelete = clinicService.FindOwnersByName(searchOwnerForDelete);

                            if (foundOwnersForDelete.Count == 0)
                            {
                                Console.WriteLine("Владельцы с такой фамилией не найдены!");
                                break;
                            }

                            Console.WriteLine("\nНайденные владельцы:");
                            foreach (Owner owner in foundOwnersForDelete)
                            {
                                Console.WriteLine($"ID: {owner.Id} | {owner.FullName} ({owner.PhoneNumber})");
                            }

                            Console.Write("Введите ID владельца: ");
                            if (!int.TryParse(Console.ReadLine(), out int ownerIdDelete))
                            {
                                Console.WriteLine("Неверный формат ID!");
                                break;
                            }

                            var selectedOwnerForDelete = foundOwnersForDelete.Find(o => o.Id == ownerIdDelete);
                            if (selectedOwnerForDelete == null)
                            {
                                Console.WriteLine("Выбранный ID не соответствует найденным владельцам!");
                                break;
                            }

                            List<Pet> ownerPetsForDelete = clinicService.GetPetsByOwnerId(ownerIdDelete);
                            if (ownerPetsForDelete.Count == 0)
                            {
                                Console.WriteLine($"У владельца {selectedOwnerForDelete.FullName} нет питомцев для удаления!");
                                break;
                            }

                            Console.WriteLine($"\nПитомцы владельца {selectedOwnerForDelete.FullName}:");
                            for (int i = 0; i < ownerPetsForDelete.Count; i++)
                            {
                                var p = ownerPetsForDelete[i];
                                Console.WriteLine($"{i + 1}. ID: {p.Id} | {p.Name} ({p.Species} - {p.Breed})");
                            }

                            Console.Write("Выберите номер питомца для удаления: ");
                            if (!int.TryParse(Console.ReadLine(), out int petDeleteIndex) || petDeleteIndex < 1 || petDeleteIndex > ownerPetsForDelete.Count)
                            {
                                Console.WriteLine("Неверный выбор!");
                                break;
                            }

                            var petToDelete = ownerPetsForDelete[petDeleteIndex - 1];

                            Console.Write($"Вы уверены, что хотите удалить питомца {petToDelete.Name}? (да/нет): ");
                            string confirmation = Console.ReadLine()?.ToLower();

                            if (confirmation == "да" || confirmation == "д" || confirmation == "y" || confirmation == "yes")
                            {
                                bool deleted = clinicService.DeletePet(petToDelete.Id);
                                Console.WriteLine(deleted ? "Питомец успешно удален." : "Ошибка при удалении питомца.");
                            }
                            else
                            {
                                Console.WriteLine("Удаление отменено.");
                            }
                            break;


                        case "5":
                            return;

                        default:
                            Console.WriteLine("Неверный выбор.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void CreateAppointment()
        {
            Console.Clear();
            Console.WriteLine("=== Запись на прием ===");

            Console.Write("Введите ФИО владельца: ");
            string ownerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                Console.WriteLine("ФИО не может быть пустым.");
                Console.ReadKey();
                return;
            }

            var owners = clinicService.FindOwnersByName(ownerName);
            if (owners.Count == 0)
            {
                Console.WriteLine("Владельцы не найдены.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите владельца:");
            for (int i = 0; i < owners.Count; i++)
                Console.WriteLine($"{i + 1}. {owners[i].FullName} ({owners[i].PhoneNumber})");

            if (!int.TryParse(Console.ReadLine(), out int ownerIndex) || ownerIndex < 1 || ownerIndex > owners.Count)
            {
                Console.WriteLine("Неверный выбор.");
                Console.ReadKey();
                return;
            }
            var selectedOwner = owners[ownerIndex - 1];

            var pets = clinicService.GetPetsByOwnerId(selectedOwner.Id);
            if (pets.Count == 0)
            {
                Console.WriteLine("У владельца нет питомцев.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите питомца:");
            for (int i = 0; i < pets.Count; i++)
                Console.WriteLine($"{i + 1}. {pets[i].Name} ({pets[i].Species} - {pets[i].Breed})");

            if (!int.TryParse(Console.ReadLine(), out int petIndex) || petIndex < 1 || petIndex > pets.Count)
            {
                Console.WriteLine("Неверный выбор.");
                Console.ReadKey();
                return;
            }
            var selectedPet = pets[petIndex - 1];

            Console.Write("Введите дату (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Неверный формат даты.");
                Console.ReadKey();
                return;
            }
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                Console.WriteLine("Воскресенье - выходной.");
                Console.ReadKey();
                return;
            }

            var vet = clinicService.GetVeterinarianByDay(date.DayOfWeek);
            if (vet == null)
            {
                Console.WriteLine("Нет врачей в этот день.");
                Console.ReadKey();
                return;
            }

            var slots = clinicService.GetAvailableTimeSlots(vet.Id, date);
            if (slots.Count == 0)
            {
                Console.WriteLine("Нет свободных временных слотов.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите время:");
            for (int i = 0; i < slots.Count; i++)
                Console.WriteLine($"{i + 1}. {slots[i]:HH:mm}");

            if (!int.TryParse(Console.ReadLine(), out int slotIndex) || slotIndex < 1 || slotIndex > slots.Count)
            {
                Console.WriteLine("Неверный выбор.");
                Console.ReadKey();
                return;
            }
            var selectedTime = slots[slotIndex - 1];

            Console.Write("Причина визита (по умолчанию: Плановый осмотр): ");
            string reason = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(reason)) reason = "Плановый осмотр";

            try
            {
                var appointment = clinicService.CreateAppointment(
                    selectedPet.Id,
                    vet.Id,
                    selectedTime,
                    selectedTime.ToString("HH:mm"),
                    reason,
                    selectedPet.Breed);

                Console.WriteLine("Запись успешно создана:");
                Console.WriteLine($"Владелец: {selectedOwner.FullName}");
                Console.WriteLine($"Питомец: {selectedPet.Name}");
                Console.WriteLine($"Врач: {vet.FullName}");
                Console.WriteLine($"Дата и время: {selectedTime:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"Причина: {reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при создании записи: " + ex.Message);
            }
            Console.ReadKey();
        }

        static void ShowVisitHistory()
        {
            Console.Clear();
            Console.WriteLine("=== История визитов питомца ===");

            Console.Write("Введите ФИО владельца: ");
            string ownerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                Console.WriteLine("ФИО не может быть пустым.");
                Console.ReadKey();
                return;
            }

            var owners = clinicService.FindOwnersByName(ownerName);
            if (owners.Count == 0)
            {
                Console.WriteLine("Владельцы не найдены.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите владельца:");
            for (int i = 0; i < owners.Count; i++)
                Console.WriteLine($"{i + 1}. {owners[i].FullName}");

            if (!int.TryParse(Console.ReadLine(), out int ownerIndex) || ownerIndex < 1 || ownerIndex > owners.Count)
            {
                Console.WriteLine("Неверный выбор.");
                Console.ReadKey();
                return;
            }
            var selectedOwner = owners[ownerIndex - 1];

            var pets = clinicService.GetPetsByOwnerId(selectedOwner.Id);
            if (pets.Count == 0)
            {
                Console.WriteLine("У владельца нет питомцев.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите питомца:");
            for (int i = 0; i < pets.Count; i++)
                Console.WriteLine($"{i + 1}. {pets[i].Name}");

            if (!int.TryParse(Console.ReadLine(), out int petIndex) || petIndex < 1 || petIndex > pets.Count)
            {
                Console.WriteLine("Неверный выбор.");
                Console.ReadKey();
                return;
            }
            var selectedPet = pets[petIndex - 1];

            var history = clinicService.GetVisitHistoryByPet(selectedPet.Id);

            Console.Clear();
            Console.WriteLine($"История визитов питомца {selectedPet.Name}:\n");

            if (history.Count == 0)
            {
                Console.WriteLine("История визитов пуста.");
            }
            else
            {
                foreach (var visit in history)
                {
                    Console.WriteLine($"Дата: {visit.VisitDate:dd.MM.yyyy HH:mm}");
                    Console.WriteLine($"Врач: {visit.VeterinarianName}");
                    Console.WriteLine($"Причина: {visit.Reason}");
                    if (!string.IsNullOrEmpty(visit.Diagnosis))
                        Console.WriteLine($"Диагноз: {visit.Diagnosis}");
                    if (!string.IsNullOrEmpty(visit.Treatment))
                        Console.WriteLine($"Лечение: {visit.Treatment}");
                    if (!string.IsNullOrEmpty(visit.Notes))
                        Console.WriteLine($"Заметки: {visit.Notes}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Console.WriteLine("Нажмите любую клавишу для возврата...");
            Console.ReadKey();
        }

        static void ShowSchedule()
        {
            Console.Clear();
            Console.WriteLine("=== Расписание врачей ===");

            Console.Write("Введите дату (гггг-мм-дд): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Неверный формат даты.");
                Console.ReadKey();
                return;
            }

            var appointments = clinicService.GetAppointmentsByDate(date);
            var veterinarians = clinicService.GetAllVeterinarians();

            Console.WriteLine($"\nРасписание на {date:dd.MM.yyyy}:\n");

            // Заголовок
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
                            var pet = clinicService.GetPetsByOwnerId(appointment.PetId);
                            var petName = clinicService.GetAllPets().Find(p => p.Id == appointment.PetId)?.Name ?? "Неизвестен";
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
            Console.WriteLine("Нажмите любую клавишу для возврата...");
            Console.ReadKey();
        }
    }
}

