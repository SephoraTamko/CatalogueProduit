using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductsController(ProductService service)
        {
            _service = service;
        }

        [HttpPut("{id}/price")]
        public IActionResult ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
        {
            _service.ChangeProductPrice(id, request.NewPrice);
            return NoContent();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductRequest request)
        {
            var id = _service.Create(request);
            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
            => Ok(_service.Get(id));

        [HttpGet]
        public IActionResult GetAll()
            => Ok(_service.GetAll());

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            _service.Update(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}