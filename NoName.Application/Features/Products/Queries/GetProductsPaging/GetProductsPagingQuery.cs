using MediatR;
using NoName.Application.Common;
using NoName.Application.Features.Products.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Application.Features.Product.Queries.GetProductsPaging
{

    public record GetProductsPagingQuery : IRequest<PagedResult<ProductViewModel>>
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public  string? LanguageId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
