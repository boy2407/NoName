using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Features.Carts.Commands.CreateCart;
using NoName.Application.Features.Carts.Commands.DeleteCart;
using NoName.Application.Features.Carts.Commands.UpdateCart;
using NoName.Application.Features.Carts.Queries.GetCartById;
using NoName.Application.Features.Carts.Queries.GetMyCart;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer,Admin,Manager,Staff")]
    public class CartsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart(CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var query = new GetMyCartQuery { UserId = userId };
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var query = new GetCartByIdQuery { Id = id, UserId = userId };
            var result = await _mediator.Send(query, ct);
            if (!result.IsSuccessed)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCartCommand command, CancellationToken ct)
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCartCommand command, CancellationToken ct)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(new DeleteCartCommand { Id = id, UserId = userId }, ct);
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
