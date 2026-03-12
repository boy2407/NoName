using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Commands.UpdateLanguage
{
    public class UpdateLanguage : IRequest<bool>
    {
        [JsonIgnore]
        public string Id { get; set; } 
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
