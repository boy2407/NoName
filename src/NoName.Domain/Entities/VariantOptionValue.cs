using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class VariantOptionValue
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
        public ProductVariant Variant { get; set; }
        public int OptionValueId { get; set; }
        public ProductOptionValue OptionValue { get; set; }
    }
}
