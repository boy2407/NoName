using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Application.Features.Product.DTOs
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string SeoAlias { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public List<int> CategoryIds { get; set; }
        public string LanguageId { get; set; }
    }
}
