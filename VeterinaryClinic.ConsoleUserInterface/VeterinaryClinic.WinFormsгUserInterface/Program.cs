using System;
using System.Windows.Forms;
using VeterinaryClinic.Core.Logic;
using DataAccessL.dapper;
using DataAccessL.Ef;
using DataAccessL;
using Microsoft.EntityFrameworkCore;
using Dto.Essence;
using Dto.Repo;

namespace VeterinaryClinic.WinFormsUserInterface
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создаем ClinicService
            var clinicService = CreateClinicService();

            // Запускаем главную форму с передачей сервиса
            Application.Run(new MainForm(clinicService));
        }

        /// <summary>
        /// Создает и настраивает сервис клиники
        /// </summary>
        private static ClinicService CreateClinicService()
        {
            // Выбери технологию: true - Dapper, false - Entity Framework
            bool useDapper = true; // или false для Entity Framework

            var connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\настя\\source\\repos\\VetClinic_Repo\\VtCl\\VeterinaryClinic.ConsoleUserInterface\\VeterinaryClinic.ConsoleUserInterface\\Database_VetCl.mdf;Integrated Security=True";

            if (useDapper)
            {
                // Настройка Dapper репозиториев
                IBaseRepository<OwnerDto> ownerRepo = new DapperOwnerRepo(connectionString);
                IBaseRepository<PetDto> petRepo = new DapperPetRepo(connectionString);
                IBaseRepository<VeterinarianDto> vetRepo = new DapperVeterinarianRepo(connectionString);
                IBaseRepository<AppointmentDto> appointmentRepo = new DapperAppointmentRepo(connectionString);
                IBaseRepository<VisitHistoryDto> visitHistoryRepo = new DapperVisitHistoryRepo(connectionString);

                return new ClinicService(ownerRepo, petRepo, vetRepo, appointmentRepo, visitHistoryRepo);
            }
            else
            {
                // Настройка EF репозиториев
                var options = new DbContextOptionsBuilder<Context>()
                    .UseSqlServer(connectionString)
                    .Options;

                var context = new Context(options);
                context.Database.EnsureCreated();

                IBaseRepository<OwnerDto> ownerRepo = new EfOwnerRepo(context);
                IBaseRepository<PetDto> petRepo = new EfPetRepo(context);
                IBaseRepository<VeterinarianDto> vetRepo = new EfVeterinarianRepo(context);
                IBaseRepository<AppointmentDto> appointmentRepo = new EfAppointmentRepo(context);
                IBaseRepository<VisitHistoryDto> visitHistoryRepo = new EfVisitHistoryRepo(context);

                return new ClinicService(ownerRepo, petRepo, vetRepo, appointmentRepo, visitHistoryRepo);
            }
        }
    }
}