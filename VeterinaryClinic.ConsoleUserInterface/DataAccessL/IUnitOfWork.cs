using DataAccessLayer_VtCl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Essence;

namespace DataAccessL
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Owner> Owners { get; }
        IRepository<Pet> Pets { get; }
        IRepository<Veterinarian> Veterinarians { get; }
        IRepository<Appointment> Appointments { get; }
        IRepository<VisitHistory> VisitHistories { get; }
        void Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly VetClinicDbContext _context;

        public UnitOfWork(VetClinicDbContext context)
        {
            _context = context;
            Owners = new EfOwnerRepo(_context);
            Pets = new EfPetRepo(_context);
            Veterinarians = new EfVeterinarianRepo(_context);
            Appointments = new EfAppointmentRepo(_context);
            VisitHistories = new EfVisitHistoryRepo(_context);
        }

        public IRepository<Owner> Owners { get; }
        public IRepository<Pet> Pets { get; }
        public IRepository<Veterinarian> Veterinarians { get; }
        public IRepository<Appointment> Appointments { get; }
        public IRepository<VisitHistory> VisitHistories { get; }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
