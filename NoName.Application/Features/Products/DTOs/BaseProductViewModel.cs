using NoName.Application.Features.Products.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.DTOs
{
    public class BaseProductViewModel<TVariant>
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public ProductTranslationViewModel ProductTranslation { get; set; }
        public List<string> CategoryNames { get; set; }
        public string ThumbnailImage { get; set; }
        public List<string> GalleryImages { get; set; }

        public List<TVariant> Variants { get; set; } = new();
    }
}
