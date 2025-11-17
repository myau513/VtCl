using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public abstract class EfBaseRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        protected readonly Context _context;

        public EfBaseRepository(Context context)
        {
            _context = context;
        }

        public void Dispose() => _context?.Dispose();

        public virtual IEnumerable<T> GetAll() => _context.Set<T>().AsNoTracking().ToList();

        public virtual T GetById(Guid id) => _context.Set<T>().Find(id);

        public virtual void Add(T item) => _context.Set<T>().Add(item);

        public virtual void Update(T item) => _context.Set<T>().Update(item);

        public virtual void Delete(Guid id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null) _context.Set<T>().Remove(entity);
        }

        public virtual void Save() => _context.SaveChanges();
    }
}