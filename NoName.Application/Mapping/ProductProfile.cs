using System.Collections.Generic;
using AutoMapper;
using NoName.Application.Features.Product.DTOs;
using NoName.Application.Features.Product.Commands.Create;
using NoName.Application.Features.Product.Queries.GetProductsPaging;

namespace NoName.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Map API DTO -> Application Command
            CreateMap<ProductCreateRequest, CreateProduct>()
                .ForMember(d => d.DateCreated, opt => opt.Ignore())
                .ForMember(d => d.IsActive, opt => opt.Ignore())
                .ForMember(d => d.CategoryIds, opt => opt.MapFrom(src => src.CategoryIds ?? new List<int>()));

            // Map paging DTO -> Query
            CreateMap<GetProductPagingRequest, GetProductPaging>();
        }
    }
}
