using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Commands.DeleteLanguage
{
    public class DeleteLanguage : IRequest<bool>
    {
        public string Id { get; set; }
        public DeleteLanguage (string id) => Id = id;
    }
}
