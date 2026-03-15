using AutoMapper;
using FluentValidation;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Features.Categories.Queries.GetCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{
    public class GetProductByIdValidator : AbstractValidator<GetProductById>
    {
        public GetProductByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        }
    }

    public class AdminGetProductByIdValidator : AbstractValidator<AdminGetProductById>
    {
        public AdminGetProductByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        }
    }
}
