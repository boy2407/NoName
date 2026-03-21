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

namespace NoName.Application.Features.Languages.Queries.GetLanguageById
{
    public class GetLanguageByIdHandler : IRequestHandler<GetLanguageByIdQuery, LanguageViewModel>
    {
        private readonly ILanguageRepository _repo;
        private readonly IMapper _mapper;

        public GetLanguageByIdHandler(ILanguageRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<LanguageViewModel> Handle(GetLanguageByIdQuery request, CancellationToken ct)
        {
            var language = await _repo.GetByIdAsync(request.Id, ct);

            if (language == null)
            {
                // Sử dụng String Interpolation để truyền Id vào thông báo lỗi
                throw new KeyNotFoundException($"Don't find this language with Id: {request.Id}");
            }

            return _mapper.Map<LanguageViewModel>(language);
        }
    }
}
