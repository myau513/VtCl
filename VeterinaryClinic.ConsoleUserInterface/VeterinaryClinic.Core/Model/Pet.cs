using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinaryClinic.Core.Models
{
    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty; // Вид: "Собака", "Кот", "Попугай"
        public string Breed { get; set; } = string.Empty;  // Порода: "Овчарка", "Сфинкс", "Волнистый"
        public int OwnerId { get; set; } // Связь с хозяином

        public Pet(int id, string name, string species, string breed, int ownerId)
        {
            Id = id;
            Name = name;
            Species = species;
            Breed = breed;
            OwnerId = ownerId;
        }

        public override string ToString()
        {
            return $"{Name} ({Species} - {Breed})";
        }
    }
}
