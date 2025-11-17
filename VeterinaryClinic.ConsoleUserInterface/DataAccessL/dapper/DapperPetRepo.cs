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
    public class DapperPetRepo : IRepository<PetDto>
    {
        private readonly string _connectionString;

        public DapperPetRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(PetDto petdto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Pets (Id, Name, Species, Breed, OwnerId) VALUES(@Id, @Name, @Species, @Breed, @OwnerId)";
                db.Execute(sqlQuery, petdto);
            }
        }

        public void Add(PetDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Pets WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public void Dispose()
        {
            // Для Dapper обычно не нужно
        }

        public IEnumerable<PetDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<PetDto>("SELECT * FROM Pets").ToList();
            }
        }

        public PetDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<PetDto>("SELECT * FROM Pets WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed
        }

        public void Update(PetDto item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Pets SET Name = @Name, Species = @Species, Breed = @Breed, OwnerId = @OwnerId WHERE Id = @Id";
                db.Execute(sqlQuery, item);
            }
        }
    }
}