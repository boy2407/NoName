using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Data.Entities
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
        public List<ProductImage> ProductImages { get; set; }
        public List<PromotionProduct> PromotionProducts { get; set; }
    }
}
