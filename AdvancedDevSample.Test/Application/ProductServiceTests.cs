using System;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Products;
using FluentAssertions;
using Moq;
using Xunit;

namespace AdvancedDevSample.Tests.Application
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _service = new ProductService(_repoMock.Object);
        }

        [Fact]
        public void Create_Should_Add_Product()
        {
            var req = new CreateProductRequest { Name = "Riz", Price = 12 };

            var id = _service.Create(req);

            _repoMock.Verify(r => r.Add(It.Is<Product>(p =>
                p.Name == "Riz" &&
                p.Price == 12
            )), Times.Once);

            id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void ChangePrice_Should_Update_Repository()
        {
            var product = new Product("Riz", 10);
            _repoMock.Setup(r => r.GetById(product.Id)).Returns(product);

            _service.ChangeProductPrice(product.Id, 20);

            product.Price.Should().Be(20);
            _repoMock.Verify(r => r.Update(product), Times.Once);
        }

        [Fact]
        public void ChangePrice_Should_Throw_If_Product_Not_Found()
        {
            _repoMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                     .Returns((Product)null);

            Action act = () => _service.ChangeProductPrice(Guid.NewGuid(), 10);

            act.Should().Throw<ApplicationServiceException>()
               .WithMessage("Produit introuvable");
        }
    }
}