using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Repo
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Возвращает все сущности
        /// </summary>
        /// <returns>Коллекция всех сущностей</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Возвращает сущность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Найденная сущность</returns>
        T GetById(Guid id);

        /// <summary>
        /// Добавляет новую сущность
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        void Add(T item);

        /// <summary>
        /// Обновляет существующую сущность
        /// </summary>
        /// <param name="item">Сущность с изменёнными данными</param>
        void Update(T item);

        /// <summary>
        /// Удаляет сущность по идентификатору
        /// </summary>
        /// <param name="id">Уникальный идентификатор сущности</param>
        void Delete(Guid id);

        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        void Save();
    }
}