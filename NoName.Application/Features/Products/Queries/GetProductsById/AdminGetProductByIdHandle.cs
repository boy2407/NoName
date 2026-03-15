using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Application.Features.Products.DTOs.Admin;
using NoName.Application.Features.Products.DTOs.Guest;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{
    public class AdminGetProductByIdHandle : IRequestHandler<AdminGetProductById, ProductAdminViewModel>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;

        public AdminGetProductByIdHandle(IProductRepository productRepository, IMapper mapper, ILanguageService languageService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<ProductAdminViewModel> Handle(AdminGetProductById request, CancellationToken ct)
        {
            var currentLang = await _languageService.GetCurrentLanguage();
            var product = await _productRepository.GetByIdWithDetailsAsync<ProductAdminViewModel>(request.Id, currentLang, ct);
            if (product == null)
            {
                throw new Exception($"Product Id does not exist: {request.Id}");
            }
            return product;
        }


    }
}
