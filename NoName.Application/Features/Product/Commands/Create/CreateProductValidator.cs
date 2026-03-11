using FluentValidation;
using NoName.Application.Features.Product.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreateProductValidator : AbstractValidator<CreateProduct>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên sản phẩm không được trống");
        RuleFor(x => x.OriginalPrice).GreaterThan(0).WithMessage("Giá phải lớn hơn 0");
    }
}