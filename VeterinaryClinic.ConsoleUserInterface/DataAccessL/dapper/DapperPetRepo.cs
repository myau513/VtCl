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
    public class DapperPetRepo : IRepository<Pet>
    {
        private readonly string _connectionString;

        public DapperPetRepo(string connectionString)
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
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pets' and xtype='U')
                    CREATE TABLE Pets (
                        Id_db UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                        Id INT NOT NULL,
                        Name NVARCHAR(255) NOT NULL,
                        Species NVARCHAR(100) NOT NULL,
                        Breed NVARCHAR(100) NOT NULL,
                        OwnerId INT NOT NULL
                    )";

                conn.Execute(sql);
            }
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public IEnumerable<Pet> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM Pets";
                return conn.Query<Pet>(sql).ToList();
            }
        }

        public Pet GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM Pets WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<Pet>(sql, new { Id = id });
            }
        }

        public void Add(Pet item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Pets (Id_db, Id, Name, Species, Breed, OwnerId)
                            VALUES (@Id_db, @Id, @Name, @Species, @Breed, @OwnerId)";
                conn.Execute(sql, item);
            }
        }

        public void Update(Pet item)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"UPDATE Pets SET Id = @Id, Name = @Name, Species = @Species, 
                            Breed = @Breed, OwnerId = @OwnerId
                            WHERE Id_db = @Id_db";
                var affected = conn.Execute(sql, item);
                if (affected == 0) throw new InvalidOperationException("Питомец не найден");
            }
        }

        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Pets WHERE Id_db = @Id";
                conn.Execute(sql, new { Id = id });
            }
        }
    }
}