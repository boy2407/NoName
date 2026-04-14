using MediatR;
using NoName.Shared.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategoriesByParentId
{
    public class GetCategoriesByParentIdQuery: IRequest<List<CategoryDto>>
    {
        public int? ParentId { get; set; }
        public string LanguageId { get; set; }

    }
}
