using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Application.Features.Product.DTOs
{
    public class ProductViewModel
    {
        public int Id { get; set; }        
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public  ProductTranslationViewModel productTranslation { get; set; }    
        public List<string> CategoryNames { get; set; }
        public string ThumbnailImage { get; set; }
        public List<string> GalleryImages { get; set; }
    }
}
