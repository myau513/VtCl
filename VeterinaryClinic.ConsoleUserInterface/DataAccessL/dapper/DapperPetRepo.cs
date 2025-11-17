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
    public class DapperPetRepo : IRepository<PetDto>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Статический конструктор для настройки обработчиков типов
        /// </summary>
        static DapperPetRepo()
        {
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        /// <summary>
        /// Инициализирует репозиторий с строкой подключения
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        public DapperPetRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Создает новую запись питомца
        /// </summary>
        /// <param name="petdto">DTO питомца для создания</param>
        public void Create(PetDto petdto)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Pets (Id, Name, Species, Breed, OwnerId) VALUES(@Id, @Name, @Species, @Breed, @OwnerId)";
                db.Execute(sqlQuery, petdto);
            }
        }

        /// <summary>
        /// Добавляет новую сущность (алиас для Create)
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        public void Add(PetDto item)
        {
            Create(item);
        }

        /// <summary>
        /// Удаляет запись питомца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи</param>
        public void Delete(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Pets WHERE Id = @id";
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
        /// Возвращает все записи питомцев
        /// </summary>
        /// <returns>Коллекция всех питомцев</returns>
        public IEnumerable<PetDto> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<PetDto>("SELECT Id, Name, Species, Breed, OwnerId FROM Pets").ToList();
            }
        }

        /// <summary>
        /// Находит запись питомца по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор питомца</param>
        /// <returns>Найденный питомец или null</returns>
        public PetDto GetById(Guid id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                return db.Query<PetDto>("SELECT Id, Name, Species, Breed, OwnerId FROM Pets WHERE Id = @id",
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
        /// Обновляет существующую запись питомца
        /// </summary>
        /// <param name="item">DTO питомца для обновления</param>
        public void Update(PetDto item)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Pets SET Name = @Name, Species = @Species, Breed = @Breed, OwnerId = @OwnerId WHERE Id = @Id";
                db.Execute(sqlQuery, item);
            }
        }
    }
}