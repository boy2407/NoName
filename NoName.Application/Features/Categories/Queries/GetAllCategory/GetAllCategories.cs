using MediatR;
using NoName.Application.Features.Categories.DTOs;
using NoName.Application.Features.Languages.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategory
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryViewModel>> {
        public string LanguageId { get; set; }
    }
}
