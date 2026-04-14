using NoName.Domain.Entities;
using NoName.Shared.DTOs.Products.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Shared.DTOs.Products.Admin
{
    public class ProductVariantAdminDto :ProductVariantDto
    {
        public decimal OriginalPrice { get; set; }
        public decimal Profit => Price - OriginalPrice;
        public decimal ProfitMargin => Price != 0 ? (Profit / Price) * 100 : 0;
        public InventoryDto Inventory { get; set; }
    }
}
