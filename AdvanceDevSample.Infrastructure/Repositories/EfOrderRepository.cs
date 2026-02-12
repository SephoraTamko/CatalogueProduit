using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Orders;
using AdvanceDevSample.Infrastructure.Entities;


namespace AdvanceDevSample.Infrastructure.Repositories
{

    public class EfOrderRepository : IOrderRepository
    {
        public void Save(Order order)
        {
            var e = new OrderEntity
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                Currency = order.Currency,
                Status = (int)order.Status,
                TaxRate = order.TaxRate,
                DiscountRate = order.DiscountRate,
                ShippingCost = order.ShippingCost,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                Lines = order.Lines.Select(l => new OrderLineEntity
                {
                    OrderId = order.Id,
                    ProductId = l.ProductId,
                    PriceAtPurchase = l.PriceAtPurchase,
                    Quantity = l.Quantity
                }).ToList()
            };

            Console.WriteLine($"[DB] Commande {order.Id} enregistrée.");
        }

        public Order GetById(Guid id) =>
            throw new NotImplementedException();
    }
}
