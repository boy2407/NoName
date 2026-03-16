using NoName.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int QuantityChange { get; set; } 
        public InventoryTransactionType Type { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public  Inventory Inventory { get; set; }
    }
}
