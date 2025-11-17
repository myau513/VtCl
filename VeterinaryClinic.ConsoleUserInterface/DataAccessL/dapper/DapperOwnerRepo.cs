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
    public class DapperOwnerRepo : IRepository<OwnerDto>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Статический конструктор для настройки обработчиков типов
        /// </summary>
        static DapperOwnerRepo()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        /// <summary>
        /// Инициализирует репозиторий с строкой подключения
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        public DapperOwnerRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Создает новую запись владельца
        /// </summary>
        /// <param name="ownerdto">DTO владельца для создания</param>
        public void Create(OwnerDto ownerdto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Owners (Id, FullName, PhoneNumber) VALUES(@Id, @FullName, @PhoneNumber)";
                db.Execute(sqlQuery, ownerdto);
            }
        }

        /// <summary>
        /// Добавляет новую сущность (алиас для Create)
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        public void Add(OwnerDto item)
        {
            Create(item);
        }

        /// <summary>
        /// Удаляет запись владельца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи</param>
        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Owners WHERE Id = @id";
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
        /// Возвращает все записи владельцев
        /// </summary>
        /// <returns>Коллекция всех владельцев</returns>
        public IEnumerable<OwnerDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<OwnerDto>("SELECT Id, FullName, PhoneNumber FROM Owners").ToList();
            }
        }

        /// <summary>
        /// Находит запись владельца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор владельца</param>
        /// <returns>Найденный владелец или null</returns>
        public OwnerDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<OwnerDto>("SELECT Id, FullName, PhoneNumber FROM Owners WHERE Id = @id",
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
        /// Обновляет существующую запись владельца
        /// </summary>
        /// <param name="item">DTO владельца для обновления</param>
        public void Update(OwnerDto item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Owners SET FullName = @FullName, PhoneNumber = @PhoneNumber WHERE Id = @Id";
                db.Execute(sqlQuery, item);
            }
        }
    }
}