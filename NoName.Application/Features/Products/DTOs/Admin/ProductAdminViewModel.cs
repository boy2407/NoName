using NoName.Application.Features.Products.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.DTOs.Admin
{
    public class ProductAdminViewModel : ProductViewModel
    {
        public new List<ProductVariantAdminViewModel> Variants { get; set; }
    }
}
