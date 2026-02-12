using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Interfaces.Products;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using static AdvancedDevSample.Application.DTOs.ChangePriceRequest;

namespace AdvancedDevSample.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }
        public void ChangeProductPrice(Guid productId, decimal newPrice)
        {
            var product = GetProduct(productId);
            product.ChangePrice(newPrice);
            _repo.Update(product);
        }

        private Product GetProduct(Guid productId)
        {
            var prod = _repo.GetById(productId);
            Console.WriteLine(prod == null ? "NULL PRODUCT" : "PRODUCT OK");
            return prod ?? throw new ApplicationServiceException("Produit introuvable", HttpStatusCode.NotFound);
        }

        public Guid Create(CreateProductRequest request)
        {
            var product = new Product(request.Name, request.Price);
            _repo.Add(product);
            return product.Id;
        }

        public void Update(Guid id, UpdateProductRequest request)
        {
            var product = _repo.GetById(id)
                ?? throw new ApplicationServiceException("Produit introuvable", HttpStatusCode.NotFound);

            product.SetName(request.Name);
            product.ChangePrice(request.Price);
            _repo.Update(product);
        }

        public void Delete(Guid id)
        {
            _repo.Delete(id);
        }
        

        public Product Get(Guid id)
        {
            return _repo.GetById(id)
                ?? throw new ApplicationServiceException("Produit introuvable", HttpStatusCode.NotFound);
        }

        public IEnumerable<Product> GetAll() => _repo.GetAll();
    }

}
