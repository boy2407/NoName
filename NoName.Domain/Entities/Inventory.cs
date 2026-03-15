using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; } 
        public int PhysicalQuantity { get; set; }
        public int ReservedQuantity { get; set; } 
        public int AvailableQuantity => PhysicalQuantity - ReservedQuantity;
        public DateTime LastUpdated { get; set; }
        public  ProductVariant ProductVariant { get; set; }
        public List<InventoryTransaction> InventoryTransactions { get; set; }
    }
}
