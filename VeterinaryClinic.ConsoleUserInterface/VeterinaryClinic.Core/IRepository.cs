using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T item);
        IEnumerable<T> ReadAll();
        T ReadById(int id);
        void Update(T item);
        void Delete(int id);
    }
}
