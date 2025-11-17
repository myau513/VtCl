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
    public class DapperOwnerRepo : IRepository<Owner>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует репозиторий владельцев
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        public DapperOwnerRepo(string connectionString = null)
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
        /// Создает базу данных и таблицу (расширенная инициализация)
        /// </summary>
        private void CreateDatabaseAndTable()
        {
            var masterConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True";

            using (var conn = new SqlConnection(masterConnectionString))
            {
                conn.Open();
                var createDbSql = @"
                    CREATE DATABASE [VeterinaryClinic] 
                    ON PRIMARY (NAME = VeterinaryClinic_Data, 
                               FILENAME = 'C:\Users\настя\source\repos\VetClinic_Repo\VtCl\VeterinaryClinic.ConsoleUserInterface\VeterinaryClinic.ConsoleUserInterface\Database_VetCl.mdf')
                    LOG ON (NAME = VeterinaryClinic_Log, 
                           FILENAME = 'C:\Users\настя\source\repos\VetClinic_Repo\VtCl\VeterinaryClinic.ConsoleUserInterface\VeterinaryClinic.ConsoleUserInterface\Database_VetCl.ldf')";
                conn.Execute(createDbSql);
            }

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"
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
        /// Получает всех владельцев
        /// </summary>
        /// <returns>Коллекция владельцев</returns>
        public IEnumerable<Owner> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM Owners";
                return conn.Query<Owner>(sql).ToList();
            }
        }

        /// <summary>
        /// Получает владельца по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор владельца</param>
        /// <returns>Владелец или null</returns>
        public Owner GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM Owners WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<Owner>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Добавляет нового владельца
        /// </summary>
        /// <param name="item">Объект владельца</param>
        public void Add(Owner item)
        {
            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Owners (Id_db, Id, FullName, PhoneNumber)
                            VALUES (@Id_db, @Id, @FullName, @PhoneNumber)";
                conn.Execute(sql, item);
            }
        }

        /// <summary>
        /// Обновляет данные владельца
        /// </summary>
        /// <param name="item">Объект владельца с обновленными данными</param>
        public void Update(Owner item)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"UPDATE Owners SET 
                            Id = @Id, 
                            FullName = @FullName, 
                            PhoneNumber = @PhoneNumber
                            WHERE Id_db = @Id_db";
                var affected = conn.Execute(sql, item);
                if (affected == 0)
                    throw new InvalidOperationException("Владелец не найден");
            }
        }

        /// <summary>
        /// Удаляет владельца по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор владельца</param>
        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Owners WHERE Id_db = @Id";
                var affected = conn.Execute(sql, new { Id = id });
                if (affected == 0)
                    throw new InvalidOperationException("Владелец не найден");
            }
        }

        /// <summary>
        /// Проверяет существование владельца
        /// </summary>
        /// <param name="id">GUID идентификатор владельца</param>
        /// <returns>True если владелец существует</returns>
        public bool Exists(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT COUNT(1) FROM Owners WHERE Id_db = @Id";
                return conn.ExecuteScalar<int>(sql, new { Id = id }) > 0;
            }
        }

        /// <summary>
        /// Получает следующий доступный числовой идентификатор
        /// </summary>
        /// <returns>Следующий доступный ID</returns>
        public int GetNextId()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Owners";
                return conn.ExecuteScalar<int>(sql);
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