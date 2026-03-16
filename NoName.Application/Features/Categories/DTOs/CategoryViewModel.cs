using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.DTOs
{
    public class CategoryViewModel
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
        public List<CategoryViewModel> ChildCategories { get; set; } = new List<CategoryViewModel>();
    }
}
