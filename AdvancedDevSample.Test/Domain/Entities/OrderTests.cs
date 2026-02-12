using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;
using Xunit;

namespace AdvancedDevSample.Test.Domain.Entities
{
    

    public class OrderTests
    {
        [Fact]
        public void TotalAmount_Calculations_AreCorrect()
        {
            var o = new Order();
            var p1 = new Product(Guid.NewGuid(), 10, true);
            var p2 = new Product(Guid.NewGuid(), 5, true);

            o.AddProduct(p1, 2);
            o.AddProduct(p2, 1);
            o.SetRates(0.2m, 0.1m, 5);

            Assert.Equal(33.6m, o.TotalAmount);
        }

        [Fact]
        public void Cannot_Modify_Paid_Order()
        {
            var o = new Order();
            var p = new Product(Guid.NewGuid(), 10, true);
            o.AddProduct(p, 1);

            o.SetStatus(OrderStatus.PendingPayment);
            o.SetStatus(OrderStatus.Paid);

            Assert.Throws<DomainException>(() => o.AddProduct(p, 1));
        }
    }
}