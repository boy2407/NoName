using MediatR;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Command.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategory, bool>
    {

        private readonly ICategoryRepository _repo;
        public DeleteCategoryHandler(ICategoryRepository repo) => _repo = repo;

        public async Task<bool> Handle(DeleteCategory request, CancellationToken ct)
        {
            var category = await _repo.GetByIdAsync(request.Id, ct);
            _repo.DeleteAsync(category, ct);
            await _repo.SaveChangesAsync(ct);
            return true;
        }
    }
}
