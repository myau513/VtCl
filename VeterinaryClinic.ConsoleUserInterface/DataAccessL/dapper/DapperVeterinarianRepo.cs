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
        static string connectionString = "data source=(localhost)\\SQLLocalDB;Initial Catalog=DbConnection;Integrated Security=True";
        IDbConnection db = new SqlConnection(connectionString);

        public void Create(VeterinarianDto veterinariandto)
        {
            var sqlQuery = "INSERT INTO Veterinarians (Id, FullName, WorkDaysString) " +
                           "VALUES(@Id, @FullName, @WorkDaysString)";
            db.Execute(sqlQuery, veterinariandto);
        }

        public void Add(VeterinarianDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            var sqlQuery = "DELETE FROM Veterinarians WHERE Id = @id";
            db.Execute(sqlQuery, new { id });
        }

        public void Dispose()
        {
            db?.Dispose();
        }

        public IEnumerable<VeterinarianDto> GetAll()
        {
            return db.Query<VeterinarianDto>("SELECT * FROM Veterinarians").ToList();
        }

        public VeterinarianDto GetById(Guid id)
        {
            return db.Query<VeterinarianDto>("SELECT * FROM Veterinarians WHERE Id = @id", new { id }).FirstOrDefault();
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed as operations are executed immediately
        }

        public void Update(VeterinarianDto item)
        {
            var sqlQuery = "UPDATE Veterinarians SET FullName = @FullName, WorkDaysString = @WorkDaysString WHERE Id = @Id";
            db.Execute(sqlQuery, item);
        }
    }
}