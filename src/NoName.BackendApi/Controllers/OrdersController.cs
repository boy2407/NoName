using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Features.Orders.Commands.CreateOrder;
using NoName.Application.Features.Orders.Commands.DeleteOrder;
using NoName.Application.Features.Orders.Commands.UpdateOrder;
using NoName.Application.Features.Orders.Commands.UpdateOrderStatus;
using NoName.Application.Features.Orders.Queries.GetMyOrders;
using NoName.Application.Features.Orders.Queries.GetOrderById;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer,Admin,Manager,Staff")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMine(CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(new GetMyOrdersQuery { UserId = userId }, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var isManagement = User.IsInRole("Admin") || User.IsInRole("Manager") || User.IsInRole("Staff");

            var query = new GetOrderByIdQuery
            {
                Id = id,
                UserId = userId,
                IsManagement = isManagement
            };

            var result = await _mediator.Send(query, ct);
            return result.IsSuccessed ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            command.UserId = userId;
            var result = await _mediator.Send(command, ct);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderCommand command, CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            command.Id = id;
            command.UserId = userId;

            var result = await _mediator.Send(command, ct);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [Authorize(policy: "ManagementContent")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusCommand command, CancellationToken ct)
        {
            command.Id = id;
            var result = await _mediator.Send(command, ct);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        [Authorize(policy: "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteOrderCommand { Id = id }, ct);
            return result.IsSuccessed ? Ok(result) : BadRequest(result);
        }

        private bool TryGetCurrentUserId(out Guid userId)
        {
            userId = Guid.Empty;
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdValue, out userId);
        }
    }
}
