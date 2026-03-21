using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Domain.Entities
{
    public class ProductTagMapping
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int TagId { get; set; }
        public ProductTag ProductTag { get; set; }

       
    }
}
