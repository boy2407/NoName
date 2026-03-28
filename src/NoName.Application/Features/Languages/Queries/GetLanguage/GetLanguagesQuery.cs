using MediatR;
using NoName.Shared.DTOs.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Queries.GetLanguage
{
    public class GetLanguagesQuery : IRequest<List<LanguageDto>> { }
}
