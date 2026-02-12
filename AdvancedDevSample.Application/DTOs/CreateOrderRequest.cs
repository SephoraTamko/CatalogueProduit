using System;
using System.Collections.Generic;
using System.Text;

namespace AdvancedDevSample.Application.DTOs
{
    public record CreateOrderRequest(
    Guid? CustomerId,
    string? Currency,
    decimal TaxRate,
    decimal DiscountRate,
    decimal ShippingCost,
    Dictionary<Guid, int> Items
);

    public record OrderResponse(
        Guid Id,
        string OrderNumber,
        decimal TotalAmount,
        string Currency,
        string Status
    );
}
