namespace NoName.Shared.DTOs.Orders
{
    public class OrderDetailDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
