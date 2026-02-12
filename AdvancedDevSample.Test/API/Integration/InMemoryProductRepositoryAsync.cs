using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Test.API.Integration
{
    public class InMemoryProductRepositoryAsync : IProductRepository
    {
        private readonly Dictionary<Guid, Product> _store= new();

        public void Add(Product product)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetByAsync(Guid id)
            =>Task.FromResult(_store.TryGetValue(id, out var p) ? p : null);

        public Product GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(Product product)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Product product)
        {
            _store[product.Id] = product;
            return Task.CompletedTask;
        }
        public void Seed(Product product)
            => _store[product.Id]= product;

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
