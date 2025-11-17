using Dapper;
using DataAccessL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL.dapper
{
    public class DapperAppointmentRepo : IRepository<Appointment>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует репозиторий с подключением к БД
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        public DapperAppointmentRepo(string connectionString = null)
        {
            _connectionString = connectionString ?? @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\настя\source\repos\VetClinic_Repo\VtCl\VeterinaryClinic.ConsoleUserInterface\VeterinaryClinic.ConsoleUserInterface\Database_VetCl.mdf;Integrated Security=True";

            try
            {
                EnsureTableCreated();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка инициализации базы данных", ex);
            }
        }

        /// <summary>
        /// Создает таблицу Owners если она не существует
        /// </summary>
        private void EnsureTableCreated()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Owners')
            CREATE TABLE Owners (
                Id_db UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                Id INT NOT NULL,
                FullName NVARCHAR(255) NOT NULL,
                PhoneNumber NVARCHAR(20) NOT NULL
            )";
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// Создает подключение к базе данных
        /// </summary>
        /// <returns>Подключение к БД</returns>
        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Получает все записи о записях на прием
        /// </summary>
        /// <returns>Коллекция всех назначений</returns>
        public IEnumerable<Appointment> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM Appointments";
                return conn.Query<Appointment>(sql).ToList();
            }
        }

        /// <summary>
        /// Получает запись на прием по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор назначения</param>
        /// <returns>Назначение или null если не найдено</returns>
        public Appointment GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM Appointments WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<Appointment>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Добавляет новую запись на прием в базу данных
        /// </summary>
        /// <param name="item">Объект назначения для добавления</param>
        public void Add(Appointment item)
        {
            if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Appointments (Id_db, Id, PetId, VeterinarianId, AppointmentDate, TimeSlot, Reason, Breed)
                            VALUES (@Id_db, @Id, @PetId, @VeterinarianId, @AppointmentDate, @TimeSlot, @Reason, @Breed)";
                conn.Execute(sql, item);
            }
        }

        /// <summary>
        /// Обновляет существующую запись на прием
        /// </summary>
        /// <param name="item">Объект назначения с обновленными данными</param>
        public void Update(Appointment item)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"UPDATE Appointments SET Id = @Id, PetId = @PetId, VeterinarianId = @VeterinarianId, 
                            AppointmentDate = @AppointmentDate, TimeSlot = @TimeSlot, Reason = @Reason, Breed = @Breed
                            WHERE Id_db = @Id_db";
                var affected = conn.Execute(sql, item);
                if (affected == 0) throw new InvalidOperationException("Запись не найдена");
            }
        }

        /// <summary>
        /// Удаляет запись на прием по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор назначения для удаления</param>
        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Appointments WHERE Id_db = @Id";
                conn.Execute(sql, new { Id = id });
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}