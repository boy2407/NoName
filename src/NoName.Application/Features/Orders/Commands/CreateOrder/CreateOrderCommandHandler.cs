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
using Microsoft.Extensions.Logging;

namespace NoName.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(
        IDistributedLockService lockService,
        IUnitOfWork unitOfWork,
        ILogger<CreateOrderCommandHandler> logger) 
        : IRequestHandler<CreateOrderCommand, ApiResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDistributedLockService _lockService = lockService;
        private readonly ILogger<CreateOrderCommandHandler> _logger = logger;

        public async Task<ApiResult<int>> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            _logger.LogInformation("CreateOrderCommand received - UserId: {UserId}, Items: {ItemCount}", 
                request.UserId, request.Items?.Count ?? 0);

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
                        _logger.LogWarning("Product variant {VariantId} not found", item.ProductVariantId);
                        return ApiResult<int>.Failure($"Product variant {item.ProductVariantId} does not exist.");
                    }

                    variantPriceMap[item.ProductVariantId] = variant.Price;

                    var availableQuantity = Math.Max(variant.Inventory?.AvailableQuantity ?? 0, 0);

                    if (availableQuantity < item.Quantity)
                    {
                        _logger.LogWarning("Insufficient stock for variant {VariantId}. Available: {Available}, Requested: {Requested}", 
                            item.ProductVariantId, availableQuantity, item.Quantity);
                        return ApiResult<int>.Failure($"Insufficient stock for variant {item.ProductVariantId}. Available quantity: {availableQuantity}.");
                    }

                    var reserved = item.Quantity;
                    if (variant.Inventory == null)
                    {
                        _logger.LogError("Variant {VariantId} has no inventory record", item.ProductVariantId);
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
                    TotalAmount = items.Sum(x => x.Quantity * variantPriceMap[x.ProductVariantId]),
                    OrderDetails = items.Select(x => new OrderDetail
                    {
                        ProductVariantId = x.ProductVariantId,
                        Quantity = x.Quantity,
                        Price = variantPriceMap[x.ProductVariantId]
                    }).ToList()
                };

                _logger.LogInformation("Creating order - UserId: {UserId}, TotalAmount: {TotalAmount}, ItemCount: {ItemCount}", 
                    order.UserId, order.TotalAmount, order.OrderDetails.Count);

                await _unitOfWork.Orders.AddAsync(order, ct);

                try
                {
                    await _unitOfWork.SaveChangesAsync(ct);
                    _logger.LogInformation("Order {OrderId} created successfully", order.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save order for UserId: {UserId}", request.UserId);
                    throw;
                }

                return ApiResult<int>.Success(order.Id, "Order created.");
            }
        }
    }
}
