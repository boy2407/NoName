using MediatR;
using NoName.Application.Common;
using NoName.Shared.DTOs.Products.Admin;
using NoName.Shared.DTOs.Products.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Update.Variants
{
    public class UpdateProductVariantCommand : IRequest<ApiResult<bool>>
    {
        public int ProductId { get; set; }
        public List<UpdateVariant> Variants { get; set; } = new();
    }
    public class UpdateVariant
    {
        public int? Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int PhysicalStock { get; set; }
    }
}
