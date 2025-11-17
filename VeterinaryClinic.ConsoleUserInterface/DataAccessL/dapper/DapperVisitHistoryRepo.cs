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
    public class DapperVisitHistoryRepo : IRepository<VisitHistoryDto>
    {
        static string connectionString = "data source=(localhost)\\SQLLocalDB;Initial Catalog=DbConnection;Integrated Security=True";
        IDbConnection db = new SqlConnection(connectionString);

        public void Create(VisitHistoryDto visithistorydto)
        {
            var sqlQuery = "INSERT INTO VisitHistory (Id, PetId, VeterinarianName, VisitDate, Reason, Diagnosis, Treatment, Notes) " +
                           "VALUES(@Id, @PetId, @VeterinarianName, @VisitDate, @Reason, @Diagnosis, @Treatment, @Notes)";
            db.Execute(sqlQuery, visithistorydto);
        }

        public void Add(VisitHistoryDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            var sqlQuery = "DELETE FROM VisitHistory WHERE Id = @id";
            db.Execute(sqlQuery, new { id });
        }

        public void Dispose()
        {
            db?.Dispose();
        }

        public IEnumerable<VisitHistoryDto> GetAll()
        {
            return db.Query<VisitHistoryDto>("SELECT * FROM VisitHistory").ToList();
        }

        public VisitHistoryDto GetById(Guid id)
        {
            return db.Query<VisitHistoryDto>("SELECT * FROM VisitHistory WHERE Id = @id", new { id }).FirstOrDefault();
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed as operations are executed immediately
        }

        public void Update(VisitHistoryDto item)
        {
            var sqlQuery = "UPDATE VisitHistory SET PetId = @PetId, VeterinarianName = @VeterinarianName, " +
                           "VisitDate = @VisitDate, Reason = @Reason, Diagnosis = @Diagnosis, " +
                           "Treatment = @Treatment, Notes = @Notes WHERE Id = @Id";
            db.Execute(sqlQuery, item);
        }
    }
}