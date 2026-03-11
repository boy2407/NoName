using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        {   //----------------Command Mapping
     
            //CreateMap<ProductCreateRequest, CreateProduct>()
            //    .ForMember(d => d.CategoryIds, opt => opt.MapFrom(src => src.CategoryIds ?? new List<int>()));

            //Map Command -> Domain Entity 
            CreateMap<CreateProduct,Product>()
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

            ///-----------------Query Mapping
  
            CreateMap<GetProductPagingRequest, GetProductPaging>();

            CreateMap<Product, ProductViewModel>()
           
              .ForMember(dest => dest.LanguageId, opt => opt.MapFrom((src, dest, destMember, context) =>
                  context.Items["LanguageId"]))

           
              .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => {
                  var lang = (string)context.Items["LanguageId"];
                  return src.ProductTranslations.FirstOrDefault(t => t.LanguageId == lang)?.Name ?? "N/A";
              }))

              .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => {
                  var lang = (string)context.Items["LanguageId"];
                  return src.ProductTranslations.FirstOrDefault(t => t.LanguageId == lang)?.Description;
              }))

              .ForMember(dest => dest.SeoAlias, opt => opt.MapFrom((src, dest, destMember, context) => {
                  var lang = (string)context.Items["LanguageId"];
                  return src.ProductTranslations.FirstOrDefault(t => t.LanguageId == lang)?.SeoAlias;
              }))

             
              .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src =>
                   src.ProductImages.FirstOrDefault(x => x.IsDefault).ImagePath ??
                   src.ProductImages.FirstOrDefault().ImagePath))

              .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src =>
                   src.ProductInCategories.Select(pc => pc.CategoryId).ToList()));
        }
    }

}
