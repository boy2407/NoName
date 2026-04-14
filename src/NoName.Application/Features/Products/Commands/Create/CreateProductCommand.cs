using MediatR;
using Microsoft.AspNetCore.Http;
using NoName.Shared.DTOs.Products.Guest;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Products.Commands.Create
{

    public class CreateProductCommand : IRequest<int>
    {
        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<ProductTranslationDto> Translations { get; set; }
        public bool IsActive { get; set; } = true;
    }
    public class AddProductVariantCommand : IRequest<bool>
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }

        public List<int> OptionValueIds { get; set; } = new List<int>();
    }
    public class AddProductImageCommand : IRequest<bool>
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public IFormFile ThumbnailImage { get; set; }
        public List<IFormFile> GalleryImages { get; set; }
    }
}
