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
    public class DapperPetRepo : IRepository<Pet>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует репозиторий питомцев
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        public DapperPetRepo(string connectionString = null)
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
        /// Получает всех питомцев
        /// </summary>
        /// <returns>Коллекция питомцев</returns>
        public IEnumerable<Pet> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM Pets";
                return conn.Query<Pet>(sql).ToList();
            }
        }

        /// <summary>
        /// Получает питомца по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор питомца</param>
        /// <returns>Питомец или null</returns>
        public Pet GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM Pets WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<Pet>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Добавляет нового питомца
        /// </summary>
        /// <param name="item">Объект питомца</param>
        public void Add(Pet item)
        {
            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Pets (Id_db, Id, Name, Species, Breed, OwnerId)
                            VALUES (@Id_db, @Id, @Name, @Species, @Breed, @OwnerId)";
                conn.Execute(sql, item);
            }
        }

        /// <summary>
        /// Обновляет данные питомца
        /// </summary>
        /// <param name="item">Объект питомца с обновленными данными</param>
        public void Update(Pet item)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"UPDATE Pets SET 
                            Id = @Id, 
                            Name = @Name, 
                            Species = @Species, 
                            Breed = @Breed, 
                            OwnerId = @OwnerId
                            WHERE Id_db = @Id_db";
                var affected = conn.Execute(sql, item);
                if (affected == 0)
                    throw new InvalidOperationException("Питомец не найден");
            }
        }

        /// <summary>
        /// Удаляет питомца по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор питомца</param>
        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Pets WHERE Id_db = @Id";
                var affected = conn.Execute(sql, new { Id = id });
                if (affected == 0)
                    throw new InvalidOperationException("Питомец не найден");
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