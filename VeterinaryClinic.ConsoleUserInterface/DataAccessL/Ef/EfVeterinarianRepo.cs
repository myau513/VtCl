using DataAccessLayer_VtCl;
using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public class EfVeterinarianRepo : IRepository<Veterinarian>
    {
        private readonly VetClinicDbContext _context;

        public EfVeterinarianRepo(VetClinicDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Veterinarian> GetAll()
        {
            return _context.Veterinarians.AsNoTracking().ToList();
        }

        public Veterinarian GetById(Guid id)
        {
            return _context.Veterinarians.Find(id);
        }

        public void Add(Veterinarian item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();
            _context.Veterinarians.Add(item);
            _context.SaveChanges();
        }

        public void Update(Veterinarian item)
        {
            var existing = _context.Veterinarians.Find(item.Id_db);
            if (existing == null) throw new InvalidOperationException("Ветеринар не найден");
            _context.Entry(existing).CurrentValues.SetValues(item);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var existing = _context.Veterinarians.Find(id);
            if (existing != null)
            {
                _context.Veterinarians.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}