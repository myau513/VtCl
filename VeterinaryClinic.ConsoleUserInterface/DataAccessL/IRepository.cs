using System;
using System.Collections.Generic;

namespace DataAccessL
{
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Возвращает все сущности
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Возвращает сущность по айди
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        T GetById(Guid id);

        /// <summary>
        /// Добавляет сущность 
        /// </summary>
        /// <param name="item">Сущность</param>
        void Add(T item);

        /// <summary>
        /// Обновляет сущность
        /// </summary>
        /// <param name="item">Сущность с изменёнными данными</param>
        void Update(T item);

        /// <summary>
        /// Удаляет сущность по айди
        /// </summary>
        /// <param name="id">Уникальный идентификатор сущности</param>
        void Delete(Guid id);

        void Save();
    }
}