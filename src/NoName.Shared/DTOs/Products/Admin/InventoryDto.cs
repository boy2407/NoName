using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Shared.DTOs.Products.Admin
{
    public class InventoryDto
    {
        public int Physical { get; set; }
        public int Reserved { get; set; }
        public int ActualAvailable { get; set; }
    }
}
