using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL.dapper
{
    public class DapperVisitHistoryRepo : IRepository<VisitHistory>
    {
        private readonly string _connectionString;

        public DapperVisitHistoryRepo(string connectionString = null)
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

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public IEnumerable<VisitHistory> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM VisitHistories";
                return conn.Query<VisitHistory>(sql).ToList();
            }
        }

        public VisitHistory GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM VisitHistories WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<VisitHistory>(sql, new { Id = id });
            }
        }

        public void Add(VisitHistory item)
        {
            if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO VisitHistories (Id_db, Id, PetId, VeterinarianName, VisitDate, Reason, Diagnosis, Treatment, Notes)
                            VALUES (@Id_db, @Id, @PetId, @VeterinarianName, @VisitDate, @Reason, @Diagnosis, @Treatment, @Notes)";
                conn.Execute(sql, item);
            }
        }

        public void Update(VisitHistory item)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"UPDATE VisitHistories SET Id = @Id, PetId = @PetId, VeterinarianName = @VeterinarianName, 
                            VisitDate = @VisitDate, Reason = @Reason, Diagnosis = @Diagnosis, Treatment = @Treatment, Notes = @Notes
                            WHERE Id_db = @Id_db";
                var affected = conn.Execute(sql, item);
                if (affected == 0) throw new InvalidOperationException("Запись истории визита не найдена");
            }
        }

        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM VisitHistories WHERE Id_db = @Id";
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