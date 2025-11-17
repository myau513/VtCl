using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Essence 
{
    public class OwnerDto : IDomainObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{FullName} ({PhoneNumber})";
        }
    }
}