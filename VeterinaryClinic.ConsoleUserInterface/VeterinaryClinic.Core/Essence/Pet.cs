using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Essence
{
    public class Pet : IDomainObject
    {
        public Guid Id_db { get; set; } = Guid.NewGuid();
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty; 
        public string Breed { get; set; } = string.Empty;  
        public int OwnerId { get; set; } 

        public Pet(int id, string name, string species, string breed, int ownerId)
        {
            Id = id;
            Name = name;
            Species = species;
            Breed = breed;
            OwnerId = ownerId;
        }

        /// <summary>
        /// Возвращает строковое представление питомца в формате "Кличка (Вид - Порода)"
        /// </summary>
        /// <returns>Строка с информацией о питомце</returns>

        public override string ToString()
        {
            return $"{Name} ({Species} - {Breed})";
        }
    }
}
