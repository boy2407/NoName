using AutoMapper;
using NoName.Application.Features.Product.Commands.Create;
using NoName.Application.Features.Product.DTOs;
using NoName.Application.Features.Product.Queries.GetProductsPaging;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoName.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            //Map API Request -> Command
            CreateMap<ProductCreateRequest, CreateProduct>()
                .ForMember(d => d.CategoryIds, opt => opt.MapFrom(src => src.CategoryIds ?? new List<int>()));

            //Map Command -> Domain Entity 
            CreateMap<CreateProduct, NoName.Domain.Entities.Product>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(_ => 0))
                // Map Translation
                .ForMember(dest => dest.ProductTranslations, opt => opt.MapFrom(src => new List<ProductTranslation>
                {
                    new ProductTranslation
                    {
                        Name = src.Name,
                        Description = src.Description,
                        Details = src.Details,
                        SeoAlias = src.SeoAlias,
                        LanguageId = src.LanguageId
                    }
                }))
                // Map Categories
                .ForMember(dest => dest.ProductInCategories, opt => opt.MapFrom(src =>
                    src.CategoryIds.Select(id => new ProductInCategory { CategoryId = id })))
                // Ignore ProductImages -> Domain Service Handle
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

            // Map Paging
            CreateMap<GetProductPagingRequest, GetProductPaging>();
        }
    }


}
