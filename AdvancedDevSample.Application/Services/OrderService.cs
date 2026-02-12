using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Products;
using System.Net;

public class OrderService
{
    private readonly IProductRepository _products;

    public OrderService(IProductRepository products)
    {
        _products = products;
    }

    public Order PlaceOrder(CreateOrderRequest req)
    {
        var order = new Order();

        if (req.CustomerId.HasValue)
            order.SetCustomer(req.CustomerId.Value);

        if (!string.IsNullOrWhiteSpace(req.Currency))
            order.SetCurrency(req.Currency!);

        order.SetRates(req.TaxRate, req.DiscountRate, req.ShippingCost);

        foreach (var item in req.Items)
        {
            var product = _products.GetById(item.Key)
                ?? throw new ApplicationServiceException("Produit introuvable", HttpStatusCode.NotFound);

            order.AddProduct(product, item.Value);
        }

        return order;
    }
}