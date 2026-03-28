using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Features.Categories.Commands.UpdateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategoryByIdWithTranslate
{
    public class GetCategoryByIdWithTranslatesQueryHandler : IRequestHandler<GetCategoryByIdWithTranslatesQuery, UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdWithTranslatesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<UpdateCategoryCommand> Handle(GetCategoryByIdWithTranslatesQuery request, CancellationToken ct)
        {   
            var category = await _categoryRepository.GetByIdWithTranslationsAsync(request.Id, ct);
            if (category == null) throw new Exception("Category does not exist.");
            return _mapper.Map<UpdateCategoryCommand>(category);
        }

    }
}
