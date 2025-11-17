using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Dto.Essence;

namespace DataAccessL
{
    public abstract class EfBaseRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        protected readonly Context _context;

        /// <summary>
        /// Инициализирует репозиторий с контекстом БД
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public EfBaseRepository(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Освобождает ресурсы контекста
        /// </summary>
        public void Dispose() => _context?.Dispose();

        /// <summary>
        /// Возвращает все сущности 
        /// </summary>
        /// <returns>Коллекция всех сущностей</returns>
        public virtual IEnumerable<T> GetAll() => _context.Set<T>().AsNoTracking().ToList();

        /// <summary>
        /// Находит сущность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Найденная сущность или null</returns>
        public virtual T GetById(Guid id) => _context.Set<T>().Find(id);

        /// <summary>
        /// Добавляет новую сущность
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        public virtual void Add(T item) => _context.Set<T>().Add(item);

        /// <summary>
        /// Обновляет существующую сущность
        /// </summary>
        /// <param name="item">Обновляемая сущность</param>
        public virtual void Update(T item) => _context.Set<T>().Update(item);

        /// <summary>
        /// Удаляет сущность по ID (проверяет существование)
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности</param>
        public virtual void Delete(Guid id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null) _context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Сохраняет все изменения в базе данных
        /// </summary>
        public virtual void Save() => _context.SaveChanges();
    }
}