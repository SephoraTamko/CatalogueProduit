using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Product() { }

        public Product(string name, decimal price)
        {
            Id = Guid.NewGuid();
            SetName(name);
            IsActive = true;
            ChangePrice(price);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }
        public Product(Guid id, string name, decimal price, bool isActive, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Price = price;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = DateTime.UtcNow;
        }

        public Product(Guid guid, int v1, bool v2)
        {
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Le nom du produit est obligatoire");

            Name = name;
            Touch();
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new DomainException("Le prix doit être positif");

            if (!IsActive)
                throw new DomainException("Produit inactif");

            Price = newPrice;
            Touch();
        }

        public void Activate() { 
            IsActive = true;
            Touch();
        }

        public void Deactivate()
        {
            IsActive = false;
            Touch();
        }

        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
