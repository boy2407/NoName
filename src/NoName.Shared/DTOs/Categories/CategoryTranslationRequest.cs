using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Shared.DTOs.Categories
{
    public class CategoryTranslationRequest
    {
        [DefaultValue("")]
        public string Name { get; set; }
        [DefaultValue("")]
        public string SeoDescription { get; set; }
        [DefaultValue("")]
        public string SeoTitle { get; set; }
        [DefaultValue("")]
        public string SeoAlias { get; set; }
        [DefaultValue("")]
        public string LanguageId { get; set; }
    }
}
