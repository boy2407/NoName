using FluentValidation;

namespace NoName.Application.Features.Products.Queries.GetProductsPaging
{
    public class GetProductsPagingValidator : AbstractValidator<GetProductsPagingQuery>
    {
        public GetProductsPagingValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("Page Index must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("You must take less than or Equal to 100 products at a time.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .When(x => x.CategoryId.HasValue)
                .WithMessage("Category does not exist");
        }
    }
}
