using NoName.Application.Features.Products.DTOs.Guest;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.DTOs.Admin
{
    public class ProductVariantAdminViewModel :ProductVariantViewModel
    {
        public decimal OriginalPrice { get; set; }
        public decimal Profit => Price - OriginalPrice;
        public decimal ProfitMargin => Price != 0 ? (Profit / Price) * 100 : 0;
        public InventoryViewModel Inventory { get; set; }
    }
}
