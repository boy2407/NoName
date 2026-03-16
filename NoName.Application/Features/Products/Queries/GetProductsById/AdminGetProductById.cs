using MediatR;
using NoName.Application.Features.Products.DTOs.Admin;
using NoName.Application.Features.Products.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{
    public class AdminGetProductById : IRequest<ProductAdminViewModel>
    {
        public int Id { get; set; }
        public AdminGetProductById(int id) => Id = id;
    }
}
