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
        public List<ProductTranslation> ProductTranslations { get; set; }

        public IReadOnlyCollection<ProductVariant> ProductVariants => _productVariants.AsReadOnly();
        private readonly List<ProductVariant> _productVariants = new List<ProductVariant>();

        //public bool? IsFeatured { get; set; }
        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Cart> Carts { get; set; }

        public List<PromotionProduct> PromotionProducts { get; set; }

        private readonly List<ProductImage> _productImages = new List<ProductImage>();
        public  IReadOnlyCollection<ProductImage> ProductImages => _productImages.AsReadOnly();
        public void AddVariant(string sku, decimal price, decimal originalPrice)
        {
            if (_productVariants.Any(v => v.SKU == sku))
                throw new Exception("SKU already exist");

            _productVariants.Add(new ProductVariant
            {
                SKU = sku,
                Price = price,
                OriginalPrice = originalPrice
            });
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
