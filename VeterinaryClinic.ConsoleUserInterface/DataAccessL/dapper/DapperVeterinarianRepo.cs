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
    public class DapperVeterinarianRepo : IRepository<Veterinarian>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует репозиторий ветеринаров
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        public DapperVeterinarianRepo(string connectionString = null)
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
        /// Создает таблицу Owners если не существует
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
        /// Получает всех ветеринаров
        /// </summary>
        /// <returns>Коллекция ветеринаров</returns>
        public IEnumerable<Veterinarian> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM Veterinarians";
                return conn.Query<Veterinarian>(sql).ToList();
            }
        }

        /// <summary>
        /// Получает ветеринара по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор ветеринара</param>
        /// <returns>Ветеринар или null</returns>
        public Veterinarian GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM Veterinarians WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<Veterinarian>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Добавляет нового ветеринара
        /// </summary>
        /// <param name="item">Объект ветеринара</param>
        public void Add(Veterinarian item)
        {
            if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Veterinarians (Id_db, Id, FullName, WorkDaysString)
                            VALUES (@Id_db, @Id, @FullName, @WorkDaysString)";
                conn.Execute(sql, item);
            }
        }

        /// <summary>
        /// Обновляет данные ветеринара
        /// </summary>
        /// <param name="item">Объект ветеринара с обновленными данными</param>
        public void Update(Veterinarian item)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"UPDATE Veterinarians SET Id = @Id, FullName = @FullName, WorkDaysString = @WorkDaysString
                            WHERE Id_db = @Id_db";
                var affected = conn.Execute(sql, item);
                if (affected == 0) throw new InvalidOperationException("Ветеринар не найден");
            }
        }

        /// <summary>
        /// Удаляет ветеринара по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор ветеринара</param>
        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Veterinarians WHERE Id_db = @Id";
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