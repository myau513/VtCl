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
    public class DapperOwnerRepo : IRepository<Owner>
    {
        private readonly string _connectionString;

        public DapperOwnerRepo(string connectionString)
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
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Owners' and xtype='U')
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

        public IEnumerable<Owner> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM Owners";
                return conn.Query<Owner>(sql).ToList();
            }
        }

        public Owner GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM Owners WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<Owner>(sql, new { Id = id });
            }
        }

        public void Add(Owner item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Owners (Id_db, Id, FullName, PhoneNumber)
                            VALUES (@Id_db, @Id, @FullName, @PhoneNumber)";
                conn.Execute(sql, item);
            }
        }

        public void Update(Owner item)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"UPDATE Owners SET Id = @Id, FullName = @FullName, PhoneNumber = @PhoneNumber
                            WHERE Id_db = @Id_db";
                var affected = conn.Execute(sql, item);
                if (affected == 0) throw new InvalidOperationException("Владелец не найден");
            }
        }

        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Owners WHERE Id_db = @Id";
                conn.Execute(sql, new { Id = id });
            }
        }
    }
}
