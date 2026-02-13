
namespace AdvanceDevSample.Infrastructure.Repositories
{
    internal class OrderLineEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
    }
}