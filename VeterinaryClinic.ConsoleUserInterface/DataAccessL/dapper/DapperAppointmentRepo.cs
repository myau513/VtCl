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
    public class DapperAppointmentRepo : IRepository<Appointment>
    {
        private readonly string _connectionString;

        public DapperAppointmentRepo(string connectionString)
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
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Appointments' and xtype='U')
                    CREATE TABLE Appointments (
                        Id_db UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                        Id INT NOT NULL,
                        PetId INT NOT NULL,
                        OwnerId INT NOT NULL,
                        VeterinarianName NVARCHAR(255) NOT NULL,
                        AppointmentDate DATETIME NOT NULL,
                        Reason NVARCHAR(MAX) NOT NULL
                    )";

                conn.Execute(sql);
            }
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public IEnumerable<Appointment> GetAll()
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM Appointments";
                return conn.Query<Appointment>(sql).ToList();
            }
        }

        public Appointment GetById(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "SELECT TOP 1 * FROM Appointments WHERE Id_db = @Id";
                return conn.QuerySingleOrDefault<Appointment>(sql, new { Id = id });
            }
        }

        public void Add(Appointment item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();

            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"INSERT INTO Appointments (Id_db, Id, PetId, OwnerId, VeterinarianName, AppointmentDate, Reason)
                            VALUES (@Id_db, @Id, @PetId, @OwnerId, @VeterinarianName, @AppointmentDate, @Reason)";
                conn.Execute(sql, item);
            }
        }

        public void Update(Appointment item)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = @"UPDATE Appointments SET Id = @Id, PetId = @PetId, OwnerId = @OwnerId, 
                            VeterinarianName = @VeterinarianName, AppointmentDate = @AppointmentDate, Reason = @Reason
                            WHERE Id_db = @Id_db";
                var affected = conn.Execute(sql, item);
                if (affected == 0) throw new InvalidOperationException("Запись о приеме не найдена");
            }
        }

        public void Delete(Guid id)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var sql = "DELETE FROM Appointments WHERE Id_db = @Id";
                conn.Execute(sql, new { Id = id });
            }
        }
    }
}
