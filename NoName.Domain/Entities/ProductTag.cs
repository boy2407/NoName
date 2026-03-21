using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class ProductTag
    {
        public int Id { get; set; }
        public string Name { get; set; } // Ví dụ: "Mùa hè", "Công sở", "Thoáng khí"
        public string TagGroup { get; set; } // Phân loại: "Vibe", "Weather", "Style"

        public List<ProductTagTranslation> TagTranslations { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<ProductTagMapping> ProductTagMappings { get; set; } = new();
    }
}
