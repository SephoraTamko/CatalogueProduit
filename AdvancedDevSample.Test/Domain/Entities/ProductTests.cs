using System;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace AdvancedDevSample.Tests.Domain
{
    public class ProductTests
    {
        [Fact]
        public void Create_Product_Should_Set_Properties_Correctly()
        {
            var p = new Product("Riz", 12);

            p.Name.Should().Be("Riz");
            p.Price.Should().Be(12);
            p.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Create_Product_Should_Throw_If_Price_Is_Invalid()
        {
            Action act = () => new Product("Riz", 0);
            
        }

        [Fact]
        public void ChangePrice_Should_Update_Price()
        {
            var p = new Product("Riz", 10);
            p.ChangePrice(15);

            p.Price.Should().Be(15);
        }

        [Fact]
        public void ChangePrice_Should_Throw_If_Price_Negative()
        {
            var p = new Product("Riz", 10);
            Action act = () => p.ChangePrice(-1);

            act.Should().Throw<DomainException>()
                .WithMessage("Le prix doit être positif");
        }

        [Fact]
        public void ChangePrice_Should_Throw_If_Inactive()
        {
            var p = new Product("Riz", 10);
            p.Deactivate();

            Action act = () => p.ChangePrice(5);

            act.Should().Throw<DomainException>()
                .WithMessage("Produit inactif");
        }

        [Fact]
        public void Activate_Should_Set_IsActive_True()
        {
            var p = new Product("Riz", 10);
            p.Deactivate();

            p.Activate();

            p.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Deactivate_Should_Set_IsActive_False()
        {
            var p = new Product("Riz", 10);
            p.Deactivate();

            p.IsActive.Should().BeFalse();
        }
    }
}