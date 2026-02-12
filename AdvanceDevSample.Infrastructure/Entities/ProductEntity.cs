using System;
using System.Collections.Generic;
using System.Text;

namespace AdvanceDevSample.Infrastructure.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }
}
