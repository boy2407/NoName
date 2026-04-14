using MediatR;
using NoName.Shared.DTOs.Products.Admin;
using NoName.Shared.DTOs.Products.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{
    public class AdminGetProductByIdQuery : IRequest<ProductAdminDto>
    {
        public int Id { get; set; }
        public AdminGetProductByIdQuery(int id) => Id = id;
    }
}
