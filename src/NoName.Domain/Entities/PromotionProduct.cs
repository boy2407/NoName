using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Domain.Entities
{
    public class PromotionProduct
    {
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
