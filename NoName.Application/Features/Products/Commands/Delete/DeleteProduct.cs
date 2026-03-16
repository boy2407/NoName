using MediatR;
using NoName.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Delete
{
    public class DeleteProduct:IRequest<ApiResult<bool>>
    {
        public int Id { get; set; }
    }
}
