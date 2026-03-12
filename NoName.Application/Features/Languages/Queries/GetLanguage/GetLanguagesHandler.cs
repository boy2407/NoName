using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Features.Languages.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Queries.GetLanguage
{
    public class GetLanguagesHandler : IRequestHandler<GetLanguages, List<LanguageViewModel>>
    {
        private readonly ILanguageRepository _repository; 
        private readonly IMapper _mapper;

        public GetLanguagesHandler(ILanguageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<LanguageViewModel>> Handle(GetLanguages request, CancellationToken ct)
        {
            var languages = await _repository.GetAllAsync(ct);
            return _mapper.Map<List<LanguageViewModel>>(languages);
        }
    }
}
