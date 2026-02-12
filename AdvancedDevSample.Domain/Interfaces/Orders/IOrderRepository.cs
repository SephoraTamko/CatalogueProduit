using AdvancedDevSample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Domain.Interfaces.Orders
{
    public interface IOrderRepository
    {
        void Save(Order order);
        Order GetById(Guid id);
    }
}
