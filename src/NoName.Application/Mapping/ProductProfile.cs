using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Features.Products.Commands.Create;
using NoName.Domain.Entities;
using NoName.Shared.DTOs.Products;
using NoName.Shared.DTOs.Products.Admin;
using NoName.Shared.DTOs.Products.Guest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NoName.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {   //----------------Command Mapping

            CreateMap<CreateProductCommand, Product>()

                .ForMember(dest => dest.ProductTranslations, opt => opt.MapFrom(src => src.Translations))
                .ForMember(dest => dest.ProductInCategories, opt => opt.MapFrom(src =>
                    src.CategoryIds.Select(id => new ProductInCategory { CategoryId = id })))

                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.ProductVariants, opt => opt.Ignore());

            CreateMap<ProductTranslationDto, ProductTranslation>().ReverseMap();

            ///-----------------Query Mapping

            string lang = null;

            CreateMap<ProductVariant, ProductVariantDto>()
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src =>
                    src.Inventory != null ? (src.Inventory.PhysicalQuantity - src.Inventory.ReservedQuantity) : 0))
                .ForMember(dest => dest.OptionValueNames, opt => opt.MapFrom(src =>
                    src.OptionValues
                        .OrderBy(ov => ov.OptionValue.OptionId)
                        .Select(ov => 
                            ov.OptionValue.ProductOptionValueTranslations
                            .Where(t => t.LanguageId == lang)
                            .Select(t => t.Name)
                            .FirstOrDefault()?? 
                            ov.OptionValue.ProductOptionValueTranslations
                            .Select(t => t.Name)
                            .FirstOrDefault())));

            CreateMap<ProductVariant, ProductVariantAdminDto>()
                    .IncludeBase<ProductVariant, ProductVariantDto>()
                    .ForMember(d => d.Inventory, o => o.MapFrom(src => src.Inventory));

            CreateMap<Inventory, InventoryDto>()
              .ForMember(dest => dest.Physical, opt => opt.MapFrom(src => src.PhysicalQuantity))
              .ForMember(dest => dest.Reserved, opt => opt.MapFrom(src => src.ReservedQuantity))
              .ForMember(dest => dest.ActualAvailable, opt => opt.MapFrom(src => src.PhysicalQuantity - src.ReservedQuantity));


            //CreateMap<Product, ProductViewModel>()
            //    .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>
            //        src.ProductVariants.Any() ? src.ProductVariants.Min(v => v.Price) : 0))
            //    .ForMember(dest => dest.Stock, opt => opt.MapFrom(src =>
            //        src.ProductVariants
            //            .Where(v => v.Inventory != null)
            //            .Sum(v => v.Inventory.PhysicalQuantity - v.Inventory.ReservedQuantity)))
            //    .ForMember(dest => dest.ProductTranslation, opt => opt.MapFrom(src =>
            //        src.ProductTranslations.FirstOrDefault(t => t.LanguageId == lang)))
            //    .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src =>
            //        src.ProductInCategories.Select(pc =>
            //        pc.Category.CategoryTranslations.FirstOrDefault(ct => ct.LanguageId == lang).Name)))
            //    .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src =>
            //         src.ProductImages.FirstOrDefault(i => i.IsDefault).ImagePath))
            //    .ForMember(dest => dest.GalleryImages, opt => opt.MapFrom(src =>
            //         src.ProductImages.Where(i => !i.IsDefault).Select(i => i.ImagePath)))
            //    .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.ProductVariants));


            //CreateMap<Product, ProductAdminViewModel>()
            // .IncludeBase<Product, ProductViewModel>()
            // .ForMember(d => d.Variants, o => o.MapFrom(src => src.ProductVariants));

            //Shadowing Property Mapping Issue
            //fix shawdowing , tránh lỗi .IncludeBase
            void MapProductBase<TDest, TVariant>(IMappingExpression<Product, TDest> map)
            where TDest : BaseProductDto<TVariant>
            {
                map.ForMember(d => d.Price, o => o.MapFrom(src => src.ProductVariants.Any() ? src.ProductVariants.Min(v => v.Price) : 0))
                   .ForMember(d => d.Stock, o => o.MapFrom(src => src.ProductVariants.Where(v => v.Inventory != null).Sum(v => v.Inventory.PhysicalQuantity - v.Inventory.ReservedQuantity)))
                   .ForMember(d => d.ProductTranslation, o => o.MapFrom(src => src.ProductTranslations.Where(t => t.LanguageId == lang).FirstOrDefault()))
                   .ForMember(d => d.CategoryNames, o => o.MapFrom(src => src.ProductInCategories.Select(pc => pc.Category.CategoryTranslations.Where(ct => ct.LanguageId == lang).Select(ct => ct.Name).FirstOrDefault())))
                   .ForMember(d => d.ThumbnailImage, o => o.MapFrom(src => src.ProductImages.Where(i => i.IsDefault).Select(i => i.ImagePath).FirstOrDefault()))
                   .ForMember(d => d.GalleryImages, o => o.MapFrom(src => src.ProductImages.Where(i => !i.IsDefault).Select(i => i.ImagePath)))
                   .ForMember(d => d.Variants, o => o.MapFrom(src => src.ProductVariants));
            }


            var guestMap = CreateMap<Product, ProductViewDto>();
            MapProductBase<ProductViewDto, ProductVariantDto>(guestMap);

            var adminMap = CreateMap<Product, ProductAdminDto>();
            MapProductBase<ProductAdminDto, ProductVariantAdminDto>(adminMap);

            CreateMap<ProductTranslation, ProductTranslationDto>();
        }
    }

}
