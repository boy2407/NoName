using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Update.Variants
{
    public class UpdateProductVariantValidator : AbstractValidator<UpdateProductVariant>
    {

        public UpdateProductVariantValidator() {

            RuleFor(x => x.Variants)
                    .NotEmpty().WithMessage("There must be at least one variant.")
                    .Must(v => v.Select(v => v.SKU).Distinct().Count() == v.Count).WithMessage("The list Variants must not contain duplicate SKU");// check duplicate SKU for the same product
        }
    }
}
