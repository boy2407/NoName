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

namespace NoName.Application.Features.Languages.Commands.CreateLanguage
{
    public class CreateLanguageHandler : IRequestHandler<CreateLanguage, string>
    {
        private readonly ILanguageRepository _repository;
        private readonly IMapper _mapper;

        public CreateLanguageHandler(ILanguageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateLanguage request, CancellationToken ct)
        {
            // Check ID EXISTED
            var isExisted = await _repository.ExistsAsync(request.Id, ct);
            if (isExisted)
            {
                // return BadRequest
                throw new Exception($"Language with ID '{request.Id}' already exists.");
            }
            var language = _mapper.Map<Language>(request);
             _repository.Add(language);
            await _repository.SaveChangesAsync(ct);

            return language.Id;
        }
    }
}
