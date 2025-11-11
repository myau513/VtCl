using DataAccessLayer_VtCl;
using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public class EfPetRepo : IRepository<Pet>
    {
        private readonly VetClinicDbContext _context;

        public EfPetRepo(VetClinicDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Pet> GetAll()
        {
            return _context.Pets.AsNoTracking().ToList();
        }

        public Pet GetById(Guid id)
        {
            return _context.Pets.Find(id);
        }

        public void Add(Pet item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();
            _context.Pets.Add(item);
            _context.SaveChanges();
        }

        public void Update(Pet item)
        {
            var existing = _context.Pets.Find(item.Id_db);
            if (existing == null) throw new InvalidOperationException("Питомец не найден");
            _context.Entry(existing).CurrentValues.SetValues(item);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var existing = _context.Pets.Find(id);
            if (existing != null)
            {
                _context.Pets.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}
