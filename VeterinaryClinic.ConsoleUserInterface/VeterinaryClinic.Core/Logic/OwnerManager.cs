using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VeterinaryClinic.Core.Models;

namespace VeterinaryClinic.Core.Logic
{
    public class OwnerManager
    {
        private List<Owner> ownersList = new List<Owner>();
        private int nextOwnerId = 1;

        public Owner CreateOwner(string fullName, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("ФИО не может быть пустым");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Номер телефона не может быть пустым");

            Owner newOwner = new Owner(nextOwnerId++, fullName.Trim(), phoneNumber.Trim());

            ownersList.Add(newOwner);
            return newOwner;
        }

        public Owner GetOwner(int id)
        {
            return ownersList.FirstOrDefault(owner => owner.Id == id);
        }

        public List<Owner> GetAllOwners()
        {
            return ownersList;
        }

        public void UpdateOwner(Owner owner)
        {
            Owner existingOwner = GetOwner(owner.Id);
            if (existingOwner == null)
                throw new ArgumentException($"Владелец с ID {owner.Id} не найден");

            if (string.IsNullOrWhiteSpace(owner.FullName))
                throw new ArgumentException("ФИО не может быть пустым");

            if (string.IsNullOrWhiteSpace(owner.PhoneNumber))
                throw new ArgumentException("Номер телефона не может быть пустым");

            existingOwner.FullName = owner.FullName.Trim();
            existingOwner.PhoneNumber = owner.PhoneNumber.Trim();
        }

        public bool DeleteOwner(int id)
        {
            Owner ownerToRemove = GetOwner(id);
            if (ownerToRemove != null)
            {
                ownersList.Remove(ownerToRemove);
                Console.WriteLine($"Владелец с ID {id} успешно удален");
                return true;
            }
            else
            {
                Console.WriteLine($"Владелец с ID {id} не найден");
                return false;
            }
        }

        public List<Owner> FindOwnersByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Owner>();

            return ownersList
                .Where(owner => owner.FullName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public bool OwnerExists(int id)
        {
            return ownersList.Any(owner => owner.Id == id);
        }
    }
}
