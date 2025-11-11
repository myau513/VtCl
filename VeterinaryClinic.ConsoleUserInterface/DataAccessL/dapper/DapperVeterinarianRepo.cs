using Dapper;
using DataAccessLayer_VtCl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public class DapperVeterinarianRepo : IRepository<Veterinarian>
    {
        private readonly string _connectionString;

        public DapperVeterinarianRepo(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTableCreated();
        }

        private void EnsureTableCreated()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();

                var sql = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Veterinarians' and xtype='U')
                    CREATE TABLE Veterinarians (
                        Id_db UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                        Id INT NOT NULL,
                        FullName NVARCHAR(255) NOT NULL,
                        WorkDaysString NVARCHAR(100) NOT NULL
                    )";

                conn.Execute(sql);
            }
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public IEnumerable<Veterinarian> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM Veterinarians";
                return conn.Query<Veterinarian>(sql).ToList();
            }
        }

        public Veterinarian GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM Veterinarians WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<Veterinarian>(sql, new { Id = id });
            }
        }

        public void Add(Veterinarian item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Veterinarians (Id_db, Id, FullName, WorkDaysString)
                            VALUES (@Id_db, @Id, @FullName, @WorkDaysString)";
                conn.Execute(sql, item);
            }
        }

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

        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Veterinarians WHERE Id_db = @Id";
                conn.Execute(sql, new { Id = id });
            }
        }
    }
}