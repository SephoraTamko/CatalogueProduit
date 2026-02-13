
namespace AdvanceDevSample.Infrastructure.Repositories
{
    internal class OrderEntity
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public Guid? CustomerId { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public object Lines { get; set; }
    }
}