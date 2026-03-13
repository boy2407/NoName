using MediatR;
using NoName.Application.Features.Categories.Command.UpdateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategoryWithTranslate
{
    public class GetCategoryByIdWithTranslates : IRequest<UpdateCategory>
    {
        public int Id { get; set; }
        public GetCategoryByIdWithTranslates(int id) => Id = id;
    }
}
