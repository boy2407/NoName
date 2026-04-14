using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Shared.DTOs.Products.Admin;
using NoName.Shared.DTOs.Products.Guest;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{
    public class AdminGetProductByIdQueryHandler : IRequestHandler<AdminGetProductByIdQuery, ProductAdminDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;

        public AdminGetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper, ILanguageService languageService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<ProductAdminDto> Handle(AdminGetProductByIdQuery request, CancellationToken ct)
        {
            var currentLang = await _languageService.GetCurrentLanguage();
            var product = await _productRepository.GetByIdWithDetailsAsync<ProductAdminDto>(request.Id, currentLang, ct);
            if (product == null)
            {
                throw new Exception($"Product Id does not exist: {request.Id}");
            }
            return product;
        }


    }
}
