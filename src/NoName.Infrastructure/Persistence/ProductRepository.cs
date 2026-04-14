using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;
using NoName.Application.Features.Products.Queries.GetProductsPaging;
using NoName.Domain.Entities;
using NoName.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoName.Shared.DTOs.Chatbot;
using NoName.Shared.DTOs.Products.Guest;

namespace NoName.Infrastructure.Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly NoNameDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(NoNameDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // --- COMMANDS ---

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            var tracked = _context.Products.Local.FirstOrDefault(p => p.Id == product.Id);
            if (tracked == null)
            {
                _context.Products.Attach(product);
                _context.Entry(product).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(tracked).CurrentValues.SetValues(product);
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Remove(product);
            await Task.CompletedTask;
        }

        // --- QUERIES

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(x => x.Id == id, cancellationToken);
        }

        //Check SKU
        public async Task<bool> CheckSkuExistsAsync(string sku, CancellationToken ct)
        {
            return await _context.ProductVariants.AnyAsync(v => v.SKU == sku, ct);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.ProductTranslations)
                .Include(p => p.ProductVariants) // Load luôn variant
                    .ThenInclude(v => v.Inventory) // Load luôn kho
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Product?> GetProductWithImagesAsync(int id, CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.ProductTranslations)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }
  
        public async Task<PagedResult<ProductViewDto>> GetProductsPagingAsync(GetProductsPagingQuery request, CancellationToken ct = default)
        {
            var query = _context.Products.AsNoTracking();

            // Filter theo ngôn ngữ và từ khóa
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                query = query.Where(p => p.ProductTranslations.Any(t =>
                    t.LanguageId == request.LanguageId && t.Name.Contains(request.Keyword)));
            }

            // Filter theo Category
            if (request.CategoryId.HasValue && request.CategoryId > 0)
            {
                query = query.Where(p => p.ProductInCategories.Any(pc => pc.CategoryId == request.CategoryId));
            }

            int totalRecords = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(p => p.DateCreated)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ProductViewDto>(_mapper.ConfigurationProvider, new { lang = request.LanguageId })
                .ToListAsync(ct);

            return new PagedResult<ProductViewDto>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        public IQueryable<Product> Query() => _context.Products.AsQueryable();
        public async Task<Product> GetProductForUpdateAsync(int id, CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.ProductInCategories) // Load danh mục để so khớp
                .Include(p => p.ProductTranslations) // Load bản dịch để so khớp
                .Include(p => p.Options) // load options
                    .ThenInclude(o => o.ProductOptionTranslations)
                .Include(p => p.Options)
                    .ThenInclude(o => o.Values)
                        .ThenInclude(v => v.ProductOptionValueTranslations) // load option values + translations
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }
        public async Task<T> GetByIdWithDetailsAsync<T>(int id, string languageId, CancellationToken cancellationToken)
        where T : class
        {
            //  ProjectTo auto include related entities 
            return await _context.Products
                .Where(p => p.Id == id)
                .ProjectTo<T>(_mapper.ConfigurationProvider, new { lang = languageId }) 
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }
        public IQueryable<Product> GetProductQuery(int id)
        {
            return _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id);
        }
        // --------------AI
        public async Task<List<Product>> SearchByAiCriteriaAsync(AiSearchCriteria criteria)
        {

            var selectedTags = criteria.Tags ?? new List<string>();
            var selectedColors = criteria.Colors ?? new List<string>();
            var selectedMaterials = criteria.Materials ?? new List<string>();
            var langCode = criteria.LanguageId ?? "vi-VN";


            // 1. Khởi tạo Base Query & Eager Loading các bảng Translation theo LanguageCode
            var query = _context.Products
                .Include(p => p.ProductTranslations.Where(t => t.LanguageId == criteria.LanguageId))
                .Include(p => p.ProductVariants)
                .Include(p => p.Options)
                    .ThenInclude(o => o.ProductOptionTranslations.Where(ot => ot.LanguageId == criteria.LanguageId))
                .Include(p => p.Options)
                    .ThenInclude(o => o.Values)
                        .ThenInclude(v => v.ProductOptionValueTranslations.Where(vt => vt.LanguageId == criteria.LanguageId))
                .AsSplitQuery()
                .AsQueryable();

            // 2. Filter theo Category (Tìm trong CategoryTranslation và ProductTranslation)
            if (!string.IsNullOrEmpty(criteria.Category))
            {
                query = query.Where(p => p.ProductInCategories.Any(pic =>
                    pic.Category.CategoryTranslations.Any(ct =>
                        ct.LanguageId == criteria.LanguageId && ct.Name.Contains(criteria.Category)))
                    ||
                    p.ProductTranslations.Any(pt =>
                        pt.LanguageId == criteria.LanguageId && pt.Name.Contains(criteria.Category))
                );
            }

            // 3. Filter theo Tags đa ngôn ngữ
            if (criteria.Tags != null && criteria.Tags.Any())
            {
                query = query.Where(p => p.ProductTagMappings.Any(ptm =>
                    ptm.ProductTag.TagTranslations.Any(ptt =>
                        ptt.LanguageId == criteria.LanguageId && criteria.Tags.Contains(ptt.Name))));
            }

            // 4. Filter theo Giá (Tối đa)
            if (criteria.MaxPrice.HasValue && criteria.MaxPrice > 0)
            {
                query = query.Where(p => p.ProductVariants.Any(pv => pv.Price <= criteria.MaxPrice.Value));
            }

            // 5. Filter theo Màu sắc (Tìm Option Name và Value Name trong bảng Translation)
            if (criteria.Colors != null && criteria.Colors.Any())
            {
                query = query.Where(p => p.Options.Any(o =>
                    o.ProductOptionTranslations.Any(ot =>
                        ot.LanguageId == criteria.LanguageId && (ot.Name.ToLower() == "color" || ot.Name.ToLower() == "màu sắc")) &&
                    o.Values.Any(v => v.ProductOptionValueTranslations.Any(vt =>
                        vt.LanguageId == criteria.LanguageId && criteria.Colors.Contains(vt.Name)))
                ));
            }

            // 6. Filter theo Chất liệu (Tương tự Màu sắc)
            if (criteria.Materials != null && criteria.Materials.Any())
            {
                query = query.Where(p => p.Options.Any(o =>
                    o.ProductOptionTranslations.Any(ot =>
                        ot.LanguageId == criteria.LanguageId && (ot.Name.ToLower() == "material" || ot.Name.ToLower() == "chất liệu")) &&
                    o.Values.Any(v => v.ProductOptionValueTranslations.Any(vt =>
                        vt.LanguageId == criteria.LanguageId && criteria.Materials.Contains(vt.Name)))
                ));
            }

            // 7. Thực thi Query, giới hạn kết quả trả về để tối ưu Token cho AI
            return await query
                .OrderByDescending(p => p.DateCreated)
                .Take(10)
                .ToListAsync();
        }

        public async Task<Product?> GetByNameAsync(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return null;
            }

            return await _context.Products
                .Include(p => p.ProductTranslations)
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(p => p.ProductTranslations.Any(t => t.Name.Contains(productName)));
        }

        // --- STORED PROCEDURE ---

        public async Task<List<Product>> SearchByAiCriteriaBySPAsync(AiSearchCriteria criteria, CancellationToken ct)
        {
            // Chuyển danh sách thành DataTable
            var colorsTable = ConvertListToDataTable(criteria.Colors ?? new List<string>());
            var materialsTable = ConvertListToDataTable(criteria.Materials ?? new List<string>());
            var tagsTable = ConvertListToDataTable(criteria.Tags ?? new List<string>());

            // Tạo SqlParameters
            var languageIdParam = new SqlParameter("@LanguageId", criteria.LanguageId ?? "vi-VN");
            var categoryParam = new SqlParameter("@Category", (object?)criteria.Category ?? DBNull.Value);
            var maxPriceParam = new SqlParameter("@MaxPrice", (object?)criteria.MaxPrice ?? DBNull.Value);

            var colorsParam = new SqlParameter("@Colors", SqlDbType.Structured)
            {
                TypeName = "[dbo].[StringTableType]",
                Value = colorsTable
            };

            var materialsParam = new SqlParameter("@Materials", SqlDbType.Structured)
            {
                TypeName = "[dbo].[StringTableType]",
                Value = materialsTable
            };

            var tagsParam = new SqlParameter("@Tags", SqlDbType.Structured)
            {
                TypeName = "[dbo].[StringTableType]",
                Value = tagsTable
            };

            // Gọi Stored Procedure
            var result = await _context.Products
                .FromSqlInterpolated($"""
                    EXEC sp_SearchProductsByAiCriteria
                        @LanguageId = {languageIdParam},
                        @Category = {categoryParam},
                        @MaxPrice = {maxPriceParam},
                        @Colors = {colorsParam},
                        @Materials = {materialsParam},
                        @Tags = {tagsParam}
                    """)
                .Include(p => p.ProductTranslations.Where(t => t.LanguageId == criteria.LanguageId))
                .Include(p => p.ProductVariants)
                .Include(p => p.Options)
                    .ThenInclude(o => o.ProductOptionTranslations.Where(ot => ot.LanguageId == criteria.LanguageId))
                .Include(p => p.Options)
                    .ThenInclude(o => o.Values)
                        .ThenInclude(v => v.ProductOptionValueTranslations.Where(vt => vt.LanguageId == criteria.LanguageId))
                .AsNoTracking()
                .ToListAsync(ct);

            return result;
        }
        private DataTable ConvertListToDataTable(List<string> list)
        {
            var table = new DataTable();
            table.Columns.Add("Value", typeof(string));

            foreach (var item in list)
            {
                table.Rows.Add(item);
            }

            return table;
        }

    }
}