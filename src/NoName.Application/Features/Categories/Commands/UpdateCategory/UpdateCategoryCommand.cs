using MediatR;
using NoName.Domain.Enums;
using NoName.Shared.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        //[JsonIgnore]
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public int? ParentId { get; set; }
        public bool IsShowOnHome { get; set; }
        public Status Status { set; get; }
        public List<CategoryTranslationRequest> Translations { get; set; }
    }
}
