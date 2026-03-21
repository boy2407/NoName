using MediatR;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Commands.UpdateLanguage
{
    public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, bool>
    {
        private readonly ILanguageRepository _repo;
        public UpdateLanguageCommandHandler(ILanguageRepository repo) { _repo = repo; }

        public async Task<bool> Handle(UpdateLanguageCommand request, CancellationToken ct)
        {
            var language = await _repo.GetByIdAsync(request.Id, ct);
            if (language == null) throw new Exception("Don't find this language");

            bool isUsed = await _repo.CheckIfLanguageIsUsedAsync(request.Id);



            language.Name = request.Name;
            language.IsDefault = request.IsDefault;

            await _repo.SaveChangesAsync(ct);
            return true;
        }
    }
}
