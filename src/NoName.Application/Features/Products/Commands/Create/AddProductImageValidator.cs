using FluentValidation;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Create
{
    public class AddProductImageValidator : AbstractValidator<AddProductImageCommand>
    {

        IProductRepository _productRepository;
        public  AddProductImageValidator(IProductRepository productRepository)
       {
            _productRepository = productRepository;

            RuleFor(x => x.ProductId)
                .MustAsync(async (id, ct) =>
                {
                    var exists = await _productRepository.ExistsAsync(id, ct);
                    return exists;
                }).WithMessage("Product does not exist.");
        }
    }
}
