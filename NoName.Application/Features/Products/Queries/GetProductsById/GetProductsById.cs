using MediatR;
using NoName.Application.Features.Products.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{
    public class GetProductById : IRequest<ProductViewModel>
    {
        public int Id { get; set; }
        public GetProductById(int id) => Id = id;
    }
}
