using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Essence
{
    public class Owner : IDomainObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Owner(Guid id, string fullName, string phoneNumber)
        {
            Id = id;
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }
        /// <summary>
        /// Возвращает строковое представление владельца в формате "Фио (Телефон)".
        /// </summary>
        /// <returns>Строковое представление владельца</returns>
        public override string ToString()
        {
            return $"{FullName} ({PhoneNumber})";
        }
    }
}
