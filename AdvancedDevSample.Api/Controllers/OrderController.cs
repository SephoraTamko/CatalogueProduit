using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderRequest request)
    {
        try
        {
            var order = _service.PlaceOrder(request);

            var response = new OrderResponse(
                order.Id,
                order.OrderNumber,
                order.TotalAmount,
                order.Currency,
                order.Status.ToString()
            );

            return CreatedAtAction(nameof(Create), new { id = order.Id }, response);
        }
        catch (ApplicationServiceException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}