using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class ProductTagTranslation
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public string LanguageId { get; set; }
        public string Name { get; set; } 
        public ProductTag ProductTag { get; set; }
        public Language Language { get; set; }
    }
}
