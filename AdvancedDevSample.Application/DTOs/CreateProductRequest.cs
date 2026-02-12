using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdvancedDevSample.Application.DTOs
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        // Required for model binding, JSON deserialization, tests, and Swagger

    }
}
