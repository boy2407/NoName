using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Commands.DeleteLanguage
{
    public class DeleteLanguageCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public DeleteLanguageCommand (string id) => Id = id;
    }
}
