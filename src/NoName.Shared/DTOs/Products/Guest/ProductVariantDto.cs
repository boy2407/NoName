using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Shared.DTOs.Products.Guest
{
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<string> OptionValueNames { get; set; } = new();
        public string OptionGroup => OptionValueNames.Count > 0 ? string.Join("-", OptionValueNames) : string.Empty;
    }
}
