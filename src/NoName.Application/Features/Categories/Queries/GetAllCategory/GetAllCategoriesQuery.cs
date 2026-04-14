using MediatR;
using NoName.Shared.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetAllCategory
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryDto>> {
        public string LanguageId { get; set; }
    }
}
