using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Command.DeleteCategory
{
    public record DeleteCategory : IRequest<bool>
    {
        public int Id { set; get; }
        public DeleteCategory(int id) => Id = id;
    }
}
