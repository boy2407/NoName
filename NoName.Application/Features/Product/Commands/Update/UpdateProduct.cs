using System;
using System.Collections.Generic;
using MediatR;

namespace NoName.Application.Features.Product.Commands.Update
{
    public class UpdateProduct : IRequest<int>
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string SeoAlias { get; set; }
        public string LanguageId { get; set; } = "vi";
        public bool IsActive { get; set; } = true;
    }
}
