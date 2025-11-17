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
        static string connectionString = "data source=(localhost)\\SQLLocalDB;Initial Catalog=DbConnection;Integrated Security=True";
        IDbConnection db = new SqlConnection(connectionString);

        public void Create(PetDto petdto)
        {
            var sqlQuery = "INSERT INTO Pets (Id, Name, Species, Breed, OwnerId) " +
                           "VALUES(@Id, @Name, @Species, @Breed, @OwnerId)";
            db.Execute(sqlQuery, petdto);
        }

        public void Add(PetDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            var sqlQuery = "DELETE FROM Pets WHERE Id = @id";
            db.Execute(sqlQuery, new { id });
        }

        public void Dispose()
        {
            db?.Dispose();
        }

        public IEnumerable<PetDto> GetAll()
        {
            return db.Query<PetDto>("SELECT * FROM Pets").ToList();
        }

        public PetDto GetById(Guid id)
        {
            return db.Query<PetDto>("SELECT * FROM Pets WHERE Id = @id", new { id }).FirstOrDefault();
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed as operations are executed immediately
        }

        public void Update(PetDto item)
        {
            var sqlQuery = "UPDATE Pets SET Name = @Name, Species = @Species, Breed = @Breed, OwnerId = @OwnerId WHERE Id = @Id";
            db.Execute(sqlQuery, item);
        }
    }
}