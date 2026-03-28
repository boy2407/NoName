using FluentValidation;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Create
{
    public class AddProductVariantValidator : AbstractValidator<AddProductVariantCommand>
    {
        IProductRepository _productRepository;
        public AddProductVariantValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.ProductId)
                .MustAsync(async (id, ct) =>
                {
                    var exists = await _productRepository.ExistsAsync(id, ct);
                    return exists;
                }).WithMessage("Product does not exist.");
            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required")
                .MustAsync(async (sku, ct) => !await _productRepository.CheckSkuExistsAsync(sku, ct))
                .WithMessage("SKU already exist in the systhem");

            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.OriginalPrice).GreaterThan(0);
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        }
    }
}
