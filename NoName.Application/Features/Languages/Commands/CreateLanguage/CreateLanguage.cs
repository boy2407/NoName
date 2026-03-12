using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Commands.CreateLanguage
{
    public class CreateLanguage : IRequest<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
