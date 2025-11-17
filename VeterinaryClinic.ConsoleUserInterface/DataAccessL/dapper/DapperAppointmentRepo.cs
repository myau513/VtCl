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

        /// <summary>
        /// Статический конструктор для настройки обработчиков типов
        /// </summary>
        static DapperAppointmentRepo()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        /// <summary>
        /// Инициализирует репозиторий с строкой подключения
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        public DapperAppointmentRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Создает новую запись назначения
        /// </summary>
        /// <param name="appointmentdto">DTO назначения для создания</param>
        public void Create(AppointmentDto appointmentdto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Appointments (Id, PetId, VeterinarianId, AppointmentDate, TimeSlot, Reason, Breed) " +
                               "VALUES(@Id, @PetId, @VeterinarianId, @AppointmentDate, @TimeSlot, @Reason, @Breed)";
                db.Execute(sqlQuery, appointmentdto);
            }
        }

        /// <summary>
        /// Добавляет новую сущность (алиас для Create)
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        public void Add(AppointmentDto item)
        {
            Create(item);
        }

        /// <summary>
        /// Удаляет запись назначения по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи</param>
        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Appointments WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }

        /// <summary>
        /// Освобождает ресурсы 
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Возвращает все записи назначений
        /// </summary>
        /// <returns>Коллекция всех назначений</returns>
        public IEnumerable<AppointmentDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<AppointmentDto>("SELECT Id, PetId, VeterinarianId, AppointmentDate, TimeSlot, Reason, Breed FROM Appointments").ToList();
            }
        }

        /// <summary>
        /// Находит запись назначения по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор назначения</param>
        /// <returns>Найденное назначение или null</returns>
        public AppointmentDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<AppointmentDto>("SELECT Id, PetId, VeterinarianId, AppointmentDate, TimeSlot, Reason, Breed FROM Appointments WHERE Id = @id",
                    new { id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// Сохраняет изменения 
        /// </summary>
        public void Save()
        {
        }

        /// <summary>
        /// Обновляет существующую запись назначения
        /// </summary>
        /// <param name="item">DTO назначения для обновления</param>
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