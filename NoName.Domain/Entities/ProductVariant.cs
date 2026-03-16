using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; } 
        public decimal Price { get; set; } 
        public decimal OriginalPrice { get; set; }
        public Product Product { get; set; }
        public  Inventory Inventory { get; set; }

    }
}
