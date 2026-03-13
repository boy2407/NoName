using AutoMapper;
using NoName.Application.Features.Categories.Command.CreateCategory;
using NoName.Application.Features.Categories.Command.UpdateCategory;
using NoName.Application.Features.Categories.DTOs;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
           

            //---command  Create/Update
            CreateMap<CategoryTranslationRequest, CategoryTranslation>();

            CreateMap<CreateCategory, Category>()
                .ForMember(dest => dest.CategoryTranslations, opt => opt.MapFrom(src => src.Translations))
                .ForMember(dest => dest.ChildCategories, opt => opt.Ignore());
            /// if mapper UpdateCategory to Category,
            /// we will ignore CategoryTranslations because it delete all translations and add new translations,
            /// but we just want update translations
            CreateMap<UpdateCategory, Category>()
                .ForMember(dest => dest.CategoryTranslations, opt => opt.Ignore());


            //---for update
            CreateMap<CategoryTranslation, CategoryTranslationRequest>();
            CreateMap<Category, UpdateCategory>()
                .ForMember(dest => dest.Translations, opt => opt.MapFrom(src => src.CategoryTranslations));


            // ---query
            CreateMap<CategoryTranslation, CategoryTranslationViewModel>();
            CreateMap<Category, CategoryViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryTranslations.FirstOrDefault() != null ? src.CategoryTranslations.FirstOrDefault().Name : "NoName" +
                "" +
                ""))
                .ForMember(dest => dest.SeoAlias, opt => opt.MapFrom(src => src.CategoryTranslations.FirstOrDefault().SeoAlias))
                .ForMember(dest => dest.SeoDescription, opt => opt.MapFrom(src => src.CategoryTranslations.FirstOrDefault().SeoDescription))
                .ForMember(dest => dest.SeoTitle, opt => opt.MapFrom(src => src.CategoryTranslations.FirstOrDefault().SeoTitle));
        }
    }
}
