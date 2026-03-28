using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Domain.Entities;
using NoName.Shared.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategoriesByParentId
{
    public class GetCategoriesByParentIdQueryHandler : IRequestHandler<GetCategoriesByParentIdQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;
        public GetCategoriesByParentIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper, ILanguageRepository languageRepository, ILanguageService languageService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _languageRepository = languageRepository;
            _languageService = languageService;
        }
        public async Task<List<CategoryDto>> Handle(GetCategoriesByParentIdQuery request, CancellationToken ct)
        {
            var currentLang = await _languageService.GetCurrentLanguage();
            request.LanguageId = string.IsNullOrEmpty(request.LanguageId) ? currentLang : request.LanguageId;
            var categories = await _categoryRepository.GetByParentIdAsync(request.ParentId, request.LanguageId, ct);
            var result = _mapper.Map<List<CategoryDto>>(categories);
            return result;
        }
    }
}
