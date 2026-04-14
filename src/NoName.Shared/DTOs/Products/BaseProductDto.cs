using NoName.Shared.DTOs.Products.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Shared.DTOs.Products
{
    public class BaseProductDto<TVariant>
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public ProductTranslationDto ProductTranslation { get; set; }
        public List<string> CategoryNames { get; set; }
        public string ThumbnailImage { get; set; }
        public List<string> GalleryImages { get; set; }

        public List<TVariant> Variants { get; set; } = new();
    }
}
