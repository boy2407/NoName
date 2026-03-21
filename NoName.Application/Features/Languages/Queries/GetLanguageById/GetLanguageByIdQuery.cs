using MediatR;
using NoName.Application.Features.Languages.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Queries.GetLanguageById
{
    public class GetLanguageByIdQuery : IRequest<LanguageViewModel>
    {
        public string Id { get; set; }
        public GetLanguageByIdQuery(string id) => Id = id;
    }
}
