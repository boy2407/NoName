using AutoMapper;
using NoName.Application.Features.Languages.Commands.CreateLanguage;
using NoName.Application.Features.Languages.Commands.UpdateLanguage;
using NoName.Domain.Entities;
using NoName.Shared.DTOs.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Mapping
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<Language, LanguageDto>();

            CreateMap<CreateLanguageCommand, Language>();

            CreateMap<UpdateLanguageCommand, Language>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
        }
    }
}
