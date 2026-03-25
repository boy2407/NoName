using System;

namespace NoName.Application.Features.Carts.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
