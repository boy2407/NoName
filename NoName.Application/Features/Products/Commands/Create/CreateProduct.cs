using MediatR;
using Microsoft.AspNetCore.Http;
using NoName.Application.Features.Product.DTOs;
using System;
using System.Collections.Generic;

namespace NoName.Application.Features.Products.Commands.Create
{
  
    public class CreateProduct : IRequest<bool>
    {
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<ProductTranslationViewModel> Translations { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class AddProductImage : IRequest<bool>
    {
        public int ProductId { get; set; }
        public IFormFile ThumbnailImage { get; set; } 
        public List<IFormFile> GalleryImages { get; set; }
    }
}
