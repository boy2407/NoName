using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Domain.Entities;
using NoName.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedLockService _lockService;
        public CreateOrderCommandHandler(IDistributedLockService lockService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _lockService = lockService;
        }

        public async Task<ApiResult<int>> Handle(CreateOrderCommand request, CancellationToken ct)
        {

            if (request.Items.Count == 0)
            {
                return ApiResult<int>.Failure("Order items are required.");
            }

            var items = request.Items
                .GroupBy(x => x.ProductVariantId)
                .Select(g => new CreateOrderItemRequest
                {
                    ProductVariantId = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .OrderBy(x=>x.ProductVariantId)
                .ToList();


            var lockKeys = items.Select(x => $"lock:variant:{x.ProductVariantId}").OrderBy(k=>k).ToList();

            using (var multiLock = await _lockService.AcquireLockAsync(lockKeys, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(1)))
            {
                var variantPriceMap = new Dictionary<int, decimal>();
                foreach (var item in items)
                {
                    var variant = await _unitOfWork.ProductVariants.GetByIdAsync(item.ProductVariantId, ct);
                    if (variant == null)
                    {
                        return ApiResult<int>.Failure($"Product variant {item.ProductVariantId} does not exist.");
                    }

                    variantPriceMap[item.ProductVariantId] = variant.Price;

                    var availableQuantity = Math.Max(variant.Inventory?.AvailableQuantity ?? 0, 0);

                    if (availableQuantity < item.Quantity)
                    {
                        return ApiResult<int>.Failure($"Insufficient stock for variant {item.ProductVariantId}. Available quantity: {availableQuantity}.");
                    }

                    var reserved = item.Quantity;
                    if (variant.Inventory == null)
                    {
                        return ApiResult<int>.Failure($"Variant {item.ProductVariantId} has no inventory record.");
                    }

                    variant.Inventory.ReservedQuantity += reserved;
                    variant.Inventory.LastUpdated = DateTime.UtcNow;
                    variant.Inventory.InventoryTransactions ??= new List<InventoryTransaction>();
                    variant.Inventory.InventoryTransactions.Add(new InventoryTransaction
                    {
                        InventoryId = variant.Inventory.Id,
                        QuantityChange = -reserved,
                        Type = InventoryTransactionType.Adjustment,
                        Description = $"Reserved {reserved} for order request (VariantId: {item.ProductVariantId}).",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = request.UserId.ToString()
                    });
                }

                var order = new Order
                {
                    UserId = request.UserId,
                    OrderDate = DateTime.UtcNow,
                    ShipName = request.ShipName,
                    ShipAddress = request.ShipAddress,
                    ShipEmail = request.ShipEmail,
                    ShipPhoneNumber = request.ShipPhoneNumber,
                    Status = OrderStatus.InProgress,
                    OrderDetails = items.Select(x => new OrderDetail
                    {
                        ProductVariantId = x.ProductVariantId,
                        Quantity = x.Quantity,
                        Price = variantPriceMap[x.ProductVariantId]
                    }).ToList()
                };

                await _unitOfWork.Orders.AddAsync(order, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                return ApiResult<int>.Success(order.Id, "Order created.");
            }
        }
    }
}
