using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Domain.Entities
{
    public class Cart
    {
        public int Id { set; get; }
        public int ProductVariantId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public Guid UserId { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public DateTime DateCreated { get; set; }
        public User User { get; set; }
    }
}
