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
    public class DapperAppointmentRepo : IRepository<AppointmentDto>
    {
        private readonly string _connectionString;

        static DapperAppointmentRepo()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        public DapperAppointmentRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(AppointmentDto appointmentdto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Appointments (Id, PetId, VeterinarianId, AppointmentDate, TimeSlot, Reason, Breed) " +
                               "VALUES(@Id, @PetId, @VeterinarianId, @AppointmentDate, @TimeSlot, @Reason, @Breed)";
                db.Execute(sqlQuery, appointmentdto);
            }
        }

        public void Add(AppointmentDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Appointments WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        public void Dispose()
        {
            // Для Dapper обычно не нужно
        }

        public IEnumerable<AppointmentDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<AppointmentDto>("SELECT Id, PetId, VeterinarianId, AppointmentDate, TimeSlot, Reason, Breed FROM Appointments").ToList();
            }
        }

        public AppointmentDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<AppointmentDto>("SELECT Id, PetId, VeterinarianId, AppointmentDate, TimeSlot, Reason, Breed FROM Appointments WHERE Id = @id",
                    new { id }).FirstOrDefault();
            }
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed
        }

        public void Update(AppointmentDto item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Appointments SET PetId = @PetId, VeterinarianId = @VeterinarianId, " +
                               "AppointmentDate = @AppointmentDate, TimeSlot = @TimeSlot, Reason = @Reason, Breed = @Breed WHERE Id = @Id";
                db.Execute(sqlQuery, item);
            }
        }
    }
}