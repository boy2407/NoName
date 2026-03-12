using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Product
{
    public class ProductCommandBase
    {
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string SeoAlias { get; set; }
        public string LanguageId { get; set; }

        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
