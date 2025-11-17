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
    public class DapperAppointmentRepo<AppointmentDto> : IRepository<AppointmentDto> where AppointmentDto : class, IDomainObject, new()
    {
        static string connectionString = "data source=(localhost)\\SQLLocalDB;Initial Catalog=DbConnection;Integrated Security=True";
        IDbConnection db = new SqlConnection(connectionString);

        public void Create(AppointmentDto appointmentdto)
        {
            var sqlQuery = "INSERT INTO Appointments (Id, PetId, VeterinarianId, AppointmentDate, TimeSlot, Reason, Breed) " +
                           "VALUES(@Id, @PetId, @VeterinarianId, @AppointmentDate, @TimeSlot, @Reason, @Breed)";
            db.Execute(sqlQuery, appointmentdto);
        }

        public void Add(AppointmentDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            var sqlQuery = "DELETE FROM Appointments WHERE Id = @id";
            db.Execute(sqlQuery, new { id });
        }

        public void Dispose()
        {
            db?.Dispose();
        }

        public IEnumerable<AppointmentDto> GetAll()
        {
            return db.Query<AppointmentDto>("SELECT * FROM Appointments").ToList();
        }

        public AppointmentDto GetById(Guid id)
        {
            return db.Query<AppointmentDto>("SELECT * FROM Appointments WHERE Id = @id", new { id }).FirstOrDefault();
        }

        public void Save()
        {
            // For Dapper, Save is typically not needed as operations are executed immediately
        }

        public void Update(AppointmentDto item)
        {
            var sqlQuery = "UPDATE Appointments SET PetId = @PetId, VeterinarianId = @VeterinarianId, " +
                           "AppointmentDate = @AppointmentDate, TimeSlot = @TimeSlot, Reason = @Reason, Breed = @Breed WHERE Id = @Id";
            db.Execute(sqlQuery, item);
        }
    }
}