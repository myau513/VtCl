using DataAccessLayer_VtCl;
using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public class EfVisitHistoryRepo : IRepository<VisitHistory>
    {
        private readonly VetClinicDbContext _context;

        public EfVisitHistoryRepo(VetClinicDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<VisitHistory> GetAll()
        {
            return _context.VisitHistories.AsNoTracking().ToList();
        }

        public VisitHistory GetById(Guid id)
        {
            return _context.VisitHistories.Find(id);
        }

        public void Add(VisitHistory item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();
            _context.VisitHistories.Add(item);
            _context.SaveChanges();
        }

        public void Update(VisitHistory item)
        {
            var existing = _context.VisitHistories.Find(item.Id_db);
            if (existing == null) throw new InvalidOperationException("Запись истории визита не найдена");
            _context.Entry(existing).CurrentValues.SetValues(item);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var existing = _context.VisitHistories.Find(id);
            if (existing != null)
            {
                _context.VisitHistories.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}