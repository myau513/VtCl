using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.DTO;

namespace VeterinaryClinic.Core.Logic
{
    public class OwnerManager
    {
        private List<OwnerDto> _owners = new List<OwnerDto>();

        public OwnerDto CreateOwner(string fullName, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("ФИО не может быть пустым");
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Номер телефона не может быть пустым");

            var owner = new OwnerDto
            {
                Id = Guid.NewGuid(),
                FullName = fullName.Trim(),
                PhoneNumber = phoneNumber.Trim()
            };

            _owners.Add(owner);
            return owner;
        }

        public List<OwnerDto> GetAllOwners() => _owners;

        public OwnerDto GetOwnerById(Guid id) => _owners.FirstOrDefault(o => o.Id == id);

        public void UpdateOwner(OwnerDto ownerDto)
        {
            var existing = GetOwnerById(ownerDto.Id);
            if (existing == null)
                throw new ArgumentException("Владелец не найден");

            existing.FullName = ownerDto.FullName.Trim();
            existing.PhoneNumber = ownerDto.PhoneNumber.Trim();
        }

        public void DeleteOwner(Guid id)
        {
            var owner = GetOwnerById(id);
            if (owner != null)
                _owners.Remove(owner);
            else
                throw new ArgumentException("Владелец не найден");
        }

        public bool OwnerExists(Guid id) => _owners.Any(o => o.Id == id);
    }
}