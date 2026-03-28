using MediatR;
using NoName.Shared.DTOs.Products.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{
    public class GetProductByIdQuery : IRequest<ProductViewDto>
    {
        public int Id { get; set; }
        public GetProductByIdQuery(int id) => Id = id;
    }
}
