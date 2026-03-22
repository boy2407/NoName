using FluentValidation;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _repo;

        public DeleteCategoryValidator(ICategoryRepository repo)
        {
            _repo = repo;
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Category is request.")
                .MustAsync(async (id, ct) =>
                {
                    var exists = await _repo.ExistsAsync(id, ct);
                    return exists;
                }).WithMessage("Category does not exist.")
                .MustAsync(async (id, ct) =>
                {
                    
                    var hasChildren = await _repo.HasChildrenAsync(id, ct);

                    return !hasChildren;
                })
                .WithMessage("pls delete children categories before.");
        }
    }
}
