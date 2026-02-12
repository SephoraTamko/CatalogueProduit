namespace AdvanceDevSample.Infrastructure.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = default!;
        public Guid? CustomerId { get; set; }
        public string Currency { get; set; } = "EUR";
        public int Status { get; set; }
        public decimal TaxRate { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<OrderLineEntity> Lines { get; set; } = new();
    }

    public class OrderLineEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
    }

}