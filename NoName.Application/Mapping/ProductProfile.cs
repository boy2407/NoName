using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Features.Product.DTOs;
using NoName.Application.Features.Product.Queries.GetProductsPaging;
using NoName.Application.Features.Products.Commands.Create;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoName.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {   //----------------Command Mapping
            
            CreateMap<CreateProduct, Product>()
            .ForMember(dest => dest.ProductTranslations, opt => opt.MapFrom(src => src.Translations))
            .ForMember(dest => dest.ProductInCategories, opt => opt.MapFrom(src => src.CategoryIds.Select(id => new ProductInCategory { CategoryId = id })))
            .ForMember(dest => dest.ProductImages, opt => opt.Ignore());
            CreateMap<ProductTranslationViewModel, ProductTranslation>();

            ///-----------------Query Mapping
            CreateMap<Product, ProductViewModel>()
            
             .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src =>
                 src.ProductImages.FirstOrDefault(i => i.IsDefault).ImagePath));

            CreateMap<ProductTranslation, ProductTranslationViewModel>();

        }
    }

}
