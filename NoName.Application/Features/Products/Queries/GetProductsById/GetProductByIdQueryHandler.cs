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
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductViewModel>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper, ILanguageService languageService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<ProductViewModel> Handle(GetProductByIdQuery request, CancellationToken ct)
        {

            var lang = await _languageService.GetCurrentLanguage();
            var product = await _productRepository.GetByIdWithDetailsAsync<ProductViewModel>(request.Id, lang, ct);
            if (product == null||!product.IsActive)
            {
                throw new Exception($"Product Id does not exist: {request.Id}");
            }

            return product;

        }

       


    }    
}
