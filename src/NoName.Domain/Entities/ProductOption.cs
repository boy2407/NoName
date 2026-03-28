using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class ProductOption
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

       
        public List<ProductOptionTranslation> ProductOptionTranslations { get; set; } = new();
        public List<ProductOptionValue> Values { get; set; } = new();
    }
}
