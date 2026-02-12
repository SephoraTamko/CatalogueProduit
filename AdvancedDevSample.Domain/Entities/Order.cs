using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; private set; }

        public string OrderNumber { get; private set; }
        public Guid? CustomerId { get; private set; }
        public string Currency { get; private set; } = "EUR";

        public OrderStatus Status { get; private set; } = OrderStatus.Draft;
        public decimal TaxRate { get; private set; }
        public decimal ShippingCost { get; private set; }
        public decimal DiscountRate { get; private set; }

        private readonly List<OrderLine> _lines = new();
        public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

        public decimal SubTotal => Round(_lines.Sum(l => l.PriceAtPurchase * l.Quantity));
        public decimal DiscountAmount => Round(SubTotal * DiscountRate);
        public decimal Taxable => Round(SubTotal - DiscountAmount + ShippingCost);
        public decimal TaxAmount => Round(Taxable * TaxRate);
        public decimal TotalAmount => Round(Taxable + TaxAmount);

        public Order()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
            OrderNumber = GenerateOrderNumber();
        }

        private static string GenerateOrderNumber() =>
            $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}".Substring(0, 20);

        private static decimal Round(decimal v) =>
            Math.Round(v, 2, MidpointRounding.AwayFromZero);

        private void Touch() => UpdatedAt = DateTime.UtcNow;

        private void EnsureModifiable()
        {
            if (Status is OrderStatus.Paid or OrderStatus.Cancelled)
                throw new DomainException("Impossible de modifier la commande.");
        }

        // -------------------- RÈGLES MÉTIER --------------------

        public void AddProduct(Product p, int qty)
        {
            EnsureModifiable();
            if (!p.IsActive) throw new DomainException("Produit inactif.");
            if (qty <= 0) throw new DomainException("Quantité invalide.");

            _lines.Add(new OrderLine(p.Id, p.Price, qty));
            Touch();
        }

        public void UpdateQuantity(Guid productId, int qty)
        {
            EnsureModifiable();

            var line = _lines.FirstOrDefault(x => x.ProductId == productId)
                ?? throw new DomainException("Ligne introuvable.");

            if (qty <= 0) throw new DomainException("Quantité invalide.");

            _lines[_lines.IndexOf(line)] = line with { Quantity = qty };
            Touch();
        }

        public void RemoveLine(Guid productId)
        {
            EnsureModifiable();
            _lines.RemoveAll(l => l.ProductId == productId);
            Touch();
        }

        public void SetCustomer(Guid id)
        {
            EnsureModifiable();
            CustomerId = id;
            Touch();
        }

        public void SetCurrency(string c)
        {
            EnsureModifiable();
            Currency = c.ToUpperInvariant();
            Touch();
        }

        public void SetRates(decimal tax, decimal discount, decimal ship)
        {
            EnsureModifiable();

            if (tax < 0) throw new DomainException("Tax négative.");
            if (discount is < 0 or > 1) throw new DomainException("Remise invalide.");
            if (ship < 0) throw new DomainException("Frais invalides.");

            TaxRate = tax;
            DiscountRate = discount;
            ShippingCost = ship;

            Touch();
        }

        public void SetStatus(OrderStatus next)
        {
            bool ok = Status switch
            {
                OrderStatus.Draft => next is OrderStatus.PendingPayment or OrderStatus.Cancelled,
                OrderStatus.PendingPayment => next is OrderStatus.Paid or OrderStatus.Cancelled,
                _ => false
            };

            if (!ok)
                throw new DomainException($"Transition interdite : {Status} → {next}");

            Status = next;
            Touch();
        }
    }

    public record OrderLine(Guid ProductId, decimal PriceAtPurchase, int Quantity);

    public enum OrderStatus { Draft, PendingPayment, Paid, Cancelled }
}