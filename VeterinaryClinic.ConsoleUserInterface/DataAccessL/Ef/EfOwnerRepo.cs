using DataAccessLayer_VtCl;
using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public class EfOwnerRepo : IRepository<Owner>
    {
        private readonly VetClinicDbContext _context;

        public EfOwnerRepo(VetClinicDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Owner> GetAll()
        {
            return _context.Owners.AsNoTracking().ToList();
        }

        public Owner GetById(Guid id)
        {
            return _context.Owners.Find(id);
        }

        public void Add(Owner item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();
            _context.Owners.Add(item);
            _context.SaveChanges();
        }

        public void Update(Owner item)
        {
            var existing = _context.Owners.Find(item.Id_db);
            if (existing == null) throw new InvalidOperationException("Владелец не найден");
            _context.Entry(existing).CurrentValues.SetValues(item);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var existing = _context.Owners.Find(id);
            if (existing != null)
            {
                _context.Owners.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
