using AutoMapper;
using NoName.Application.Features.Languages.Commands.CreateLanguage;
using NoName.Application.Features.Languages.Commands.UpdateLanguage;
using NoName.Application.Features.Languages.DTOs;
using NoName.Domain.Entities;
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
            CreateMap<Language, LanguageViewModel>();

            CreateMap<CreateLanguage, Language>();

            CreateMap<UpdateLanguage, Language>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
        }
    }
}
