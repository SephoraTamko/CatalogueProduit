using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdvancedDevSample.Application.DTOs
{
    public record UpdateProductRequest(
        [Required] string Name,
        [Range(0.01, double.MaxValue)] decimal Price
    );
}
