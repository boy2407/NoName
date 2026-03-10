using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;
using NoName.Application.Features.Product.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Product.Queries.GetProductsPaging
{
    public class GetProductPagingHandle : IRequestHandler<GetProductPaging, PageResult<ProductViewModel>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; 
        public GetProductPagingHandle(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper= mapper;
        }
        public async Task<PageResult<ProductViewModel>> Handle(GetProductPaging request,CancellationToken cancellationToken)
        {
            var languageId = request.LanguageId ?? "vi-VN";

            var query = _productRepository
                .Query()
                .AsNoTracking()
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.Trim();
                query = query.Where(p =>
                    p.ProductTranslations.Any(t => t.LanguageId == languageId && t.Name.Contains(keyword)) ||
                    p.ProductInCategories.Any(pc => pc.Category.CategoryTranslations.Any(ct => ct.LanguageId == languageId && ct.Name.Contains(keyword)))
                );
            }
             
            //if (request.CategoryId.HasValue)
            //{
            //    query = query.Where(p => p.ProductInCategories.Any(pc => pc.CategoryId == request.CategoryId));
            //}

            var totalRecords = await query.CountAsync(cancellationToken);

            var pagedData = await query
            .Include(x => x.ProductTranslations)
            .Include(x => x.ProductImages)
            .Include(x => x.ProductInCategories)
            .OrderByDescending(x => x.DateCreated) 
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

            var items = _mapper.Map<List<ProductViewModel>>(pagedData, opt => {
                opt.Items["LanguageId"] = languageId;
            });

            return new PageResult<ProductViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

    }

}

