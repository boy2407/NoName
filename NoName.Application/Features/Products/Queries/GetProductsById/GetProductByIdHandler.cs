using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Features.Product.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Queries.GetProductsById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductById, ProductViewModel>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService; // Inject Service vào đây

        public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper, ILanguageService languageService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<ProductViewModel> Handle(GetProductById request, CancellationToken ct)
        {
 
            var currentLang = await _languageService.GetCurrentLanguage();
            var productViewModel = await _productRepository.GetByIdWithDetailsAsync(request.Id, currentLang, ct);
            if (productViewModel == null) return null;
            return productViewModel;
        }
    }
}
