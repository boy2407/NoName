using MediatR;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Commands.DeleteLanguage
{
    public class DeleteLanguageHandler : IRequestHandler<DeleteLanguage, bool>
    {
        private readonly ILanguageRepository _repository;

        public DeleteLanguageHandler(ILanguageRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteLanguage request, CancellationToken ct)
        {
            var language = await _repository.GetByIdAsync(request.Id, ct);

            if (language == null)
            {
                throw new KeyNotFoundException($"Cannot delete. Language with Id: {request.Id} not found.");
            }

            if (language.IsDefault)
            {
                throw new Exception("Cannot delete the default language.");
            }

            bool isUsed = await _repository.CheckIfLanguageIsUsedAsync(request.Id);

            if (isUsed)
            {
                throw new Exception("Cannot delete this language because it is being used in product or category translations.");
            }

            _repository.Delete(language);
            await _repository.SaveChangesAsync(ct);

            return true;
        }
    }
}
