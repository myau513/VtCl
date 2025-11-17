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
    public class DapperVeterinarianRepo : IRepository<VeterinarianDto>
    {
        private readonly string _connectionString;

        static DapperVeterinarianRepo()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        public DapperVeterinarianRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(VeterinarianDto veterinariandto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Veterinarians (Id, FullName, WorkDaysString) VALUES(@Id, @FullName, @WorkDaysString)";
                db.Execute(sqlQuery, veterinariandto);
            }
        }

        public void Add(VeterinarianDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Veterinarians WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public void Dispose()
        {
            // Для Dapper обычно не нужно
        }

        public IEnumerable<VeterinarianDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<VeterinarianDto>("SELECT Id, FullName, WorkDaysString FROM Veterinarians").ToList();
            }
        }

        public VeterinarianDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<VeterinarianDto>("SELECT Id, FullName, WorkDaysString FROM Veterinarians WHERE Id = @id",
                    new { id }).FirstOrDefault();
            }
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed
        }

        public void Update(VeterinarianDto item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Veterinarians SET FullName = @FullName, WorkDaysString = @WorkDaysString WHERE Id = @Id";
                db.Execute(sqlQuery, item);
            }
        }
    }
}