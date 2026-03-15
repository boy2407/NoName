using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Common;
using NoName.Application.Features.Product.Queries.GetProductsPaging;
using NoName.Application.Features.Products.Commands.Create;
using NoName.Application.Features.Products.Commands.Update.common;
using NoName.Application.Features.Products.DTOs.Admin;
using NoName.Application.Features.Products.DTOs.Guest;
using NoName.Application.Features.Products.Queries.GetProductsById;

namespace NoName.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // API - ADMIN
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProduct command)
        {
            var productId = await _mediator.Send(command);
            return Ok(new { Id = productId });
        }

        [HttpPost("{id}/variants")]
        public async Task<IActionResult> AddVariant(int id, [FromBody] AddProductVariant command)
        {
            command.ProductId = id;
            var result = await _mediator.Send(command);
            if (!result) return BadRequest("");
            return Ok();
        }


        [HttpPost("{id}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImages(int id, [FromForm] AddProductImage command)
        {
            command.ProductId = id;
            var result = await _mediator.Send(command);
            if (!result) return BadRequest("Could not upload images for this product.");
            return Ok("Images uploaded successfully.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<bool>>> Update(int id, [FromBody] UpdateProduct command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (User.IsInRole("Admin"))
            {
                var adminquery = new AdminGetProductById(id);
                return Ok(await _mediator.Send(adminquery));
            }

            var guestQuery = new GetProductById(id);
            return Ok(await _mediator.Send(guestQuery));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetProductsPagingRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
    