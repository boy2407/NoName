using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Application.Features.Product.DTOs
{
    public class GetProductPagingRequest 
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public string LanguageId { get; set; } = "vi"; // default language is vietnamese 
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
