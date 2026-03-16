using MediatR;
using NoName.Application.Features.Categories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategoriesByParentId
{
    public class GetCategoriesByParentId: IRequest<List<CategoryViewModel>>
    {
        public int? ParentId { get; set; }
        public string LanguageId { get; set; }

    }
}
