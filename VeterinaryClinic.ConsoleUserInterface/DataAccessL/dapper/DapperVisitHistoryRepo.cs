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

        /// <summary>
        /// Статический конструктор для настройки обработчиков типов
        /// </summary>
        static DapperVisitHistoryRepo()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        /// <summary>
        /// Инициализирует репозиторий с строкой подключения
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        public DapperVisitHistoryRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Создает новую запись истории посещений
        /// </summary>
        /// <param name="visithistorydto">DTO истории посещений для создания</param>
        public void Create(VisitHistoryDto visithistorydto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO VisitHistories (Id, PetId, VeterinarianName, VisitDate, Reason, Diagnosis, Treatment, Notes) " +
                               "VALUES(@Id, @PetId, @VeterinarianName, @VisitDate, @Reason, @Diagnosis, @Treatment, @Notes)";
                db.Execute(sqlQuery, visithistorydto);
            }
        }

        /// <summary>
        /// Добавляет новую сущность (алиас для Create)
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        public void Add(VisitHistoryDto item)
        {
            Create(item);
        }

        /// <summary>
        /// Удаляет запись истории посещений по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи</param>
        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM VisitHistories WHERE Id = @id";
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
        /// Возвращает все записи истории посещений
        /// </summary>
        /// <returns>Коллекция всех записей истории посещений</returns>
        public IEnumerable<VisitHistoryDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<VisitHistoryDto>("SELECT Id, PetId, VeterinarianName, VisitDate, Reason, Diagnosis, Treatment, Notes FROM VisitHistories").ToList();
            }
        }

        /// <summary>
        /// Находит запись истории посещений по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор записи истории посещений</param>
        /// <returns>Найденная запись истории посещений или null</returns>
        public VisitHistoryDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<VisitHistoryDto>("SELECT Id, PetId, VeterinarianName, VisitDate, Reason, Diagnosis, Treatment, Notes FROM VisitHistories WHERE Id = @id",
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
        /// Обновляет существующую запись истории посещений
        /// </summary>
        /// <param name="item">DTO истории посещений для обновления</param>
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