using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class ProductOptionTranslation
    {
        public int Id { set; get; }
        public int OptionId { set; get; }
        public string LanguageId { set; get; }
        public string Name { set; get; }
        public ProductOption Option { get; set; }
    }
}
