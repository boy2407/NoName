using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Features.Product.Queries.GetProductsPaging;
using NoName.Application.Features.Products.Commands.Create;
using NoName.Application.Features.Products.DTOs.Admin;
using NoName.Application.Features.Products.DTOs.Guest;
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
                .ForMember(dest => dest.ProductInCategories, opt => opt.MapFrom(src =>
                    src.CategoryIds.Select(id => new ProductInCategory { CategoryId = id })))

                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.ProductVariants, opt => opt.Ignore());

            CreateMap<ProductTranslationViewModel, ProductTranslation>().ReverseMap();

            ///-----------------Query Mapping

            string lang = null;
            CreateMap<Product, ProductViewModel>()

                .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>
                    src.ProductVariants.Any() ? src.ProductVariants.Min(v => v.Price) : 0))

                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src =>
                    src.ProductVariants
                        .Where(v => v.Inventory != null)
                        .Sum(v => v.Inventory.PhysicalQuantity - v.Inventory.ReservedQuantity)))

                .ForMember(dest => dest.ProductTranslation, opt => opt.MapFrom(src =>
                    src.ProductTranslations.FirstOrDefault(t => t.LanguageId == lang)))

                .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src =>
                    src.ProductInCategories.Select(pc =>
                    pc.Category.CategoryTranslations.FirstOrDefault(ct => ct.LanguageId == lang).Name)))

                .ForMember(dest => dest.ThumbnailImage, opt => opt.MapFrom(src =>
                     src.ProductImages.FirstOrDefault(i => i.IsDefault).ImagePath))
                .ForMember(dest => dest.GalleryImages, opt => opt.MapFrom(src =>
                     src.ProductImages.Where(i => !i.IsDefault).Select(i => i.ImagePath)))

                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.ProductVariants));

            CreateMap<ProductVariant, ProductVariantViewModel>()
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src =>
                    src.Inventory != null ?(src.Inventory.PhysicalQuantity - src.Inventory.ReservedQuantity): 0));

            CreateMap<ProductTranslation, ProductTranslationViewModel>();



            //Admin Mapping
            CreateMap<Product, ProductAdminViewModel>()
                .IncludeBase<Product, ProductViewModel>()
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.ProductVariants));

            CreateMap<ProductVariant, ProductVariantAdminViewModel>();

            CreateMap<Inventory, InventoryViewModel>()
                .ForMember(dest => dest.Physical, opt => opt.MapFrom(src => src.PhysicalQuantity))
                .ForMember(dest => dest.Reserved, opt => opt.MapFrom(src => src.ReservedQuantity))
                .ForMember(dest => dest.ActualAvailable, opt => opt.MapFrom(src => src.PhysicalQuantity - src.ReservedQuantity));
        }
    }

}
