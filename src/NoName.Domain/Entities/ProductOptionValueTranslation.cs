using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class ProductOptionValueTranslation
    {
        public int Id { set; get; }
        public int ProductOptionValueId { set; get; }
        public string LanguageId { set; get; }
        public string Name { set; get; }
        public ProductOptionValue ProductOptionValue { get; set; }
    }
}
