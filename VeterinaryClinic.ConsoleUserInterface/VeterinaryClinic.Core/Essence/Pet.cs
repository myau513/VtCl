using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Essence
{
    public class Pet : IDomainObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Species} - {Breed})";
        }
    }
}
