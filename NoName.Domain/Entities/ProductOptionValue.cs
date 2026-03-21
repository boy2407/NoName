using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class ProductOptionValue
    {
        public int Id { get; set; }
        public int OptionId { get; set; }
        public ProductOption Option { get; set; }
        public List<ProductOptionValueTranslation> ProductOptionValueTranslations { get; set; } = new();
    }
}
