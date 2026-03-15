using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Application.Features.Products.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Product.Queries.GetProductsPaging
{
    public class GetProductsPagingHandle : IRequestHandler<GetProductsPagingRequest, PagedResult<ProductViewModel>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; 
        private readonly ILanguageService _languageService;
        public GetProductsPagingHandle(IProductRepository productRepository, IMapper mapper, ILanguageService languageService)
        {
            _productRepository = productRepository;
            _mapper= mapper;
            _languageService = languageService;
        }


        public async Task<PagedResult<ProductViewModel>> Handle(GetProductsPagingRequest request, CancellationToken cancellationToken)
        {

          
            var currentLang = await _languageService.GetCurrentLanguage();
            if(request.LanguageId == null)
            {
                request.LanguageId = currentLang;
            }
            return await _productRepository.GetProductsPagingAsync(request, cancellationToken);

        }

    }

}

