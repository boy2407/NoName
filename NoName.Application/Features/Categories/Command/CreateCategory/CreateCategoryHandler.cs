using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Command.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategory, int>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageRepository _languageRepository;

        public CreateCategoryHandler (ICategoryRepository categoryRepository, IMapper mapper, ILanguageRepository languageRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _languageRepository = languageRepository;
        }

        public async Task<int> Handle(CreateCategory request, CancellationToken ct)
        {
            var category = _mapper.Map<Category>(request);
            await _categoryRepository.CreateAsync(category, ct);
            await _categoryRepository.SaveChangesAsync(ct);
            return category.Id;
        }
    }
}
