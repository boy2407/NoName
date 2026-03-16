using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Application.Features.Products.DTOs.Guest
{
    public class ProductViewModel
    {
        public int Id { get; set; }        
        public decimal Price { get; set; }
        public int Stock { get; set; } // total stock across all variants
        public bool IsActive { get; set; }
        public  ProductTranslationViewModel ProductTranslation { get; set; }    
        public List<string> CategoryNames { get; set; }
        public string ThumbnailImage { get; set; }
        public List<string> GalleryImages { get; set; }
        public List<ProductVariantViewModel> Variants { get; set; }
    }
}
