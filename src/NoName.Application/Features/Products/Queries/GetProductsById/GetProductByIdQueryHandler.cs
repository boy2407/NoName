using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Shared.DTOs.Products.Admin;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoName.Shared.DTOs.Products.Guest;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductViewDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILanguageService _languageService;

        public GetProductByIdQueryHandler(IProductRepository productRepository, ILanguageService languageService)
        {
            _productRepository = productRepository;
            _languageService = languageService;
        }

        public async Task<ProductViewDto> Handle(GetProductByIdQuery request, CancellationToken ct)
        {

            var lang = await _languageService.GetCurrentLanguage();
            var product = await _productRepository.GetByIdWithDetailsAsync<ProductViewDto>(request.Id, lang, ct);
            if (product == null||!product.IsActive)
            {
                throw new Exception($"Product Id does not exist: {request.Id}");
            }

            return product;

        }


    }    
}
