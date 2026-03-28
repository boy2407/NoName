using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoName.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int ViewCount { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DateModified { get; set; }
        public List<ProductTranslation> ProductTranslations { get; set; } = new();


        // Relationships
        public List<ProductTagMapping> ProductTagMappings { get; set; } = new();
        public List<ProductInCategory> ProductInCategories { get; set; } = new();

        public List<PromotionProduct> PromotionProducts { get; set; } = new();

        private readonly List<ProductImage> _productImages = new List<ProductImage>();

        public List<ProductTag> ProductTags { get; set; } = new();

        // Options and Variants
        public List<ProductOption> Options { get; set; } = new();

        public IReadOnlyCollection<ProductVariant> ProductVariants => _productVariants.AsReadOnly();
        private readonly List<ProductVariant> _productVariants = new List<ProductVariant>();

        public IReadOnlyCollection<ProductImage> ProductImages => _productImages.AsReadOnly();
        public void AddVariant(string sku, decimal price, decimal originalPrice, List<int> optionValueIds)
        {

            if (_productVariants.Any(v => v.SKU == sku))
            {
                throw new InvalidOperationException($"SKU '{sku}' đã tồn tại cho sản phẩm này.");
            }

            if (optionValueIds == null || !optionValueIds.Any())
            {
                throw new ArgumentException("Một biến thể sản phẩm phải có ít nhất một thuộc tính (Option Value).");
            }
            var variant = new ProductVariant
            {
                SKU = sku,
                Price = price,
                OriginalPrice = originalPrice,
                CreatedAt = DateTime.Now,
                Inventory = new Inventory
                {
                    PhysicalQuantity = 0,
                    ReservedQuantity = 0,
                    LastUpdated = DateTime.Now
                },
                OptionValues = optionValueIds.Select(id => new VariantOptionValue
                {
                    OptionValueId = id
                }).ToList()
            };
            _productVariants.Add(variant);
        }
        public void AddImage(string path, long fileSize, bool isDefault, string caption = "")
        {

            if (isDefault)
            {
                foreach (var img in _productImages)
                {
                    img.IsDefault = false;
                }
            }

            if (!_productImages.Any())
            {
                isDefault = true;
            }

            _productImages.Add(new ProductImage
            {
                ImagePath = path,
                FileSize = fileSize,
                IsDefault = isDefault,
                Caption = caption,
                DateCreated = DateTime.Now,
                SortOrder = isDefault ? 1 : 2
            });
        }
    }
}
