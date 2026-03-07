using System.Collections.Generic;
using MediatR;
using NoName.Application.Features.Product.DTOs;

namespace NoName.Application.Features.Product.Queries.GetProductsByCategory
{
    public class GetProductsByCategory : IRequest<List<ProductViewModel>>
    {
        public int CategoryId { get; set; }
        public string LanguageId { get; set; } = "vi";
    }
}
