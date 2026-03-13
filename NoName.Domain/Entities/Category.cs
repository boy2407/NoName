using NoName.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace NoName.Domain.Entities
{
    public class Category
    {
        public int Id { set; get; }
        public int SortOrder { set; get; }
        public bool IsShowOnHome { set; get; }
        public int? ParentId { set; get; }
        public Status Status { set; get; }

        /// Family: A category can have a parent category and multiple child categories.
        public Category ParentCategory { get; set; }
        public List<Category> ChildCategories { get; set; } = new List<Category>();

        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<CategoryTranslation> CategoryTranslations { get; set; } = new List<CategoryTranslation>();
    }
}
