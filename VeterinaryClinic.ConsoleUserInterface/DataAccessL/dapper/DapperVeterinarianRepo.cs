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
        private readonly string _connectionString;

        /// <summary>
        /// Статический конструктор для настройки обработчиков типов
        /// </summary>
        static DapperVeterinarianRepo()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        /// <summary>
        /// Инициализирует репозиторий с строкой подключения
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        public DapperVeterinarianRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Создает новую запись ветеринара
        /// </summary>
        /// <param name="veterinariandto">DTO ветеринара для создания</param>
        public void Create(VeterinarianDto veterinariandto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Veterinarians (Id, FullName, WorkDaysString) VALUES(@Id, @FullName, @WorkDaysString)";
                db.Execute(sqlQuery, veterinariandto);
            }
        }

        /// <summary>
        /// Добавляет новую сущность (алиас для Create)
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        public void Add(VeterinarianDto item)
        {
            Create(item);
        }

        /// <summary>
        /// Удаляет запись ветеринара по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи</param>
        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Veterinarians WHERE Id = @id";
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
        /// Возвращает все записи ветеринаров
        /// </summary>
        /// <returns>Коллекция всех ветеринаров</returns>
        public IEnumerable<VeterinarianDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<VeterinarianDto>("SELECT Id, FullName, WorkDaysString FROM Veterinarians").ToList();
            }
        }

        /// <summary>
        /// Находит запись ветеринара по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор ветеринара</param>
        /// <returns>Найденный ветеринар или null</returns>
        public VeterinarianDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<VeterinarianDto>("SELECT Id, FullName, WorkDaysString FROM Veterinarians WHERE Id = @id",
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
        /// Обновляет существующую запись ветеринара
        /// </summary>
        /// <param name="item">DTO ветеринара для обновления</param>
        public void Update(VeterinarianDto item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Veterinarians SET FullName = @FullName, WorkDaysString = @WorkDaysString WHERE Id = @Id";
                db.Execute(sqlQuery, item);
            }
        }
    }
}