using DataAccessLayer_VtCl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public class EfAppointmentRepo : IRepository<Appointment>
    {
        private readonly VetClinicDbContext _context;

        public EfAppointmentRepo(VetClinicDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _context.Appointments.AsNoTracking().ToList();
        }

        public Appointment GetById(Guid id)
        {
            return _context.Appointments.Find(id);
        }

        public void Add(Appointment item)
        {
            if (item.Id_db == Guid.Empty) item.Id_db = Guid.NewGuid();
            _context.Appointments.Add(item);
            _context.SaveChanges();
        }

        public void Update(Appointment item)
        {
            var existing = _context.Appointments.Find(item.Id_db);
            if (existing == null) throw new InvalidOperationException("Запись не найдена");
            _context.Entry(existing).CurrentValues.SetValues(item);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var existing = _context.Appointments.Find(id);
            if (existing != null)
            {
                _context.Appointments.Remove(existing);
                _context.SaveChanges();
            }
        }
    }
}

