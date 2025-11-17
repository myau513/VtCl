using System;

namespace Dto.Essence
{
    public class PetDto : IDomainObject
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