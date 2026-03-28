using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Shared.DTOs.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Queries.GetLanguage
{
    public class GetLanguagesQueryHandler : IRequestHandler<GetLanguagesQuery, List<LanguageDto>>
    {
        private readonly ILanguageRepository _repository; 
        private readonly IMapper _mapper;

        public GetLanguagesQueryHandler(ILanguageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<LanguageDto>> Handle(GetLanguagesQuery request, CancellationToken ct)
        {
            var languages = await _repository.GetAllAsync(ct);
            return _mapper.Map<List<LanguageDto>>(languages);
        }
    }
}
