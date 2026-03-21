using MediatR;
using NoName.Application.Features.Categories.DTOs;
using NoName.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Command.CreateCategory
{
    public class CreateCategoryCommand : IRequest<int>
    {

        public int SortOrder { get; set; } = 1;
        public bool IsShowOnHome { get; set; } = true;
        public int? ParentId { get; set; }
        public Status Status { get; set; }
        public List<CategoryTranslationRequest> Translations { get; set; }
    }
}
