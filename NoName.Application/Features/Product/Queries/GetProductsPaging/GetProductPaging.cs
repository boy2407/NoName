using MediatR;
using NoName.Application.Common;
using NoName.Application.Features.Product.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Application.Features.Product.Queries.GetProductsPaging
{

    public class GetProductPaging : IRequest<PageResult<ProductViewModel>>
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public string LanguageId { get; set; } = "vi"; 
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    
}
