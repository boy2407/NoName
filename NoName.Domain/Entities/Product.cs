using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoName.Domain.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }
        public bool IsActive { get; set; } = true;
        public DateTime? DateModified { get; set; }

        //public bool? IsFeatured { get; set; }
        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        //public object ProductImages { get; internal set; }
        public List<Cart> Carts { get; set; }
        public List<ProductTranslation> ProductTranslations { get; set; }
        public List<PromotionProduct> PromotionProducts { get; set; }


        private readonly List<ProductImage> _productImages = new List<ProductImage>();
        public virtual IReadOnlyCollection<ProductImage> ProductImages => _productImages.AsReadOnly();

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
