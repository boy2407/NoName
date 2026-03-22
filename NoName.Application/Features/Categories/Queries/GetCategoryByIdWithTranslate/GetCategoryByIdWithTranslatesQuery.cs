using MediatR;
using NoName.Application.Features.Categories.Commands.UpdateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategoryByIdWithTranslate
{
    public class GetCategoryByIdWithTranslatesQuery : IRequest<UpdateCategoryCommand>
    {
        public int Id { get; set; }
        public GetCategoryByIdWithTranslatesQuery(int id) => Id = id;
    }
}
