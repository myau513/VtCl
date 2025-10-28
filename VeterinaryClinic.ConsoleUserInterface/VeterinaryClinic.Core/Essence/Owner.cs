using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Essence
{
    public class Owner
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public Owner(int id, string fullName, string phoneNumber)
        {
            Id = id;
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }

        /// <summary>
        /// Возвращает строковое представление владельца в формате "ФИО (телефон)"
        /// </summary>
        /// <returns>Строка с информацией о владельце</returns>
        
        public override string ToString()
        {
            return $"{FullName} ({PhoneNumber})";
        }
    }
}
