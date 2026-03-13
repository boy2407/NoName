using MediatR;
using NoName.Application.Features.Categories.DTOs;
using NoName.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Command.UpdateCategory
{
    public class UpdateCategory : IRequest<bool>
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
