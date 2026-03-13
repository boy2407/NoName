using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Features.Categories.DTOs;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategoriesByParentId
{
    public class GetCategoriesByParentIdHandle : IRequestHandler<GetCategoriesByParentId, List<CategoryViewModel>>
    {
        ICategoryRepository _categoryRepository;
        ILanguageRepository _languageRepository; 
        IMapper _mapper;
        public GetCategoriesByParentIdHandle(ICategoryRepository categoryRepository, IMapper mapper, ILanguageRepository languageRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _languageRepository = languageRepository;
        }
        public async Task<List<CategoryViewModel>> Handle(GetCategoriesByParentId request, CancellationToken ct)
        {
            var categories = await _categoryRepository.GetByParentIdAsync(request.ParentId, request.LanguageId, ct);
            var result = _mapper.Map<List<CategoryViewModel>>(categories);
            return result;
        }
    }
}
