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
        private readonly string _connectionString;

        static DapperVisitHistoryRepo()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        public DapperVisitHistoryRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(VisitHistoryDto visithistorydto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO VisitHistories (Id, PetId, VeterinarianName, VisitDate, Reason, Diagnosis, Treatment, Notes) " +
                               "VALUES(@Id, @PetId, @VeterinarianName, @VisitDate, @Reason, @Diagnosis, @Treatment, @Notes)";
                db.Execute(sqlQuery, visithistorydto);
            }
        }

        public void Add(VisitHistoryDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM VisitHistories WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public void Dispose()
        {
            // Для Dapper обычно не нужно
        }

        public IEnumerable<VisitHistoryDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<VisitHistoryDto>("SELECT Id, PetId, VeterinarianName, VisitDate, Reason, Diagnosis, Treatment, Notes FROM VisitHistories").ToList();
            }
        }

        public VisitHistoryDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<VisitHistoryDto>("SELECT Id, PetId, VeterinarianName, VisitDate, Reason, Diagnosis, Treatment, Notes FROM VisitHistories WHERE Id = @id",
                    new { id }).FirstOrDefault();
            }
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed
        }

        public void Update(VisitHistoryDto item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE VisitHistories SET PetId = @PetId, VeterinarianName = @VeterinarianName, " +
                               "VisitDate = @VisitDate, Reason = @Reason, Diagnosis = @Diagnosis, " +
                               "Treatment = @Treatment, Notes = @Notes WHERE Id = @Id";
                db.Execute(sqlQuery, item);
            }
        }
    }
}