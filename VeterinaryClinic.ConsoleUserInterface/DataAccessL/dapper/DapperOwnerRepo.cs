using Dapper;
using DataAccessL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dto.Essence;

namespace DataAccessL.dapper
{
    public class DapperOwnerRepo : IRepository<OwnerDto>
    {
        private readonly string _connectionString;

        public DapperOwnerRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(OwnerDto ownerdto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Owners (Id, FullName, PhoneNumber) VALUES(@Id, @FullName, @PhoneNumber)";
                db.Execute(sqlQuery, ownerdto);
            }
        }

        public void Add(OwnerDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Owners WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public void Dispose()
        {
            // Для Dapper обычно не нужно
        }

        public IEnumerable<OwnerDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<OwnerDto>("SELECT * FROM Owners").ToList();
            }
        }

        public OwnerDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<OwnerDto>("SELECT * FROM Owners WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed
        }

        public void Update(OwnerDto item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Owners SET FullName = @FullName, PhoneNumber = @PhoneNumber WHERE Id = @Id";
                db.Execute(sqlQuery, item);
            }
        }
    }
}