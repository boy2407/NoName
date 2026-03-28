using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Shared.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int SortOrder { get; set; }
        public bool IsShowOnHome { get; set; }

        // Information from CategoryTranslation
        public string Name { get; set; }
        public string SeoAlias { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public List<CategoryDto> ChildCategories { get; set; } = new List<CategoryDto>();
    }
}
