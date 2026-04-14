using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Domain.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { set; get; }
        public int ProductVariantId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public Order Order { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
