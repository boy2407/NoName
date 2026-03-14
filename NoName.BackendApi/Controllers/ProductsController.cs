using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Features.Product.DTOs;
using NoName.Application.Features.Product.Queries.GetProductsPaging;
using NoName.Application.Features.Products.Commands.Create;
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
        [HttpPost("create-product")]

        public async Task<IActionResult> Create([FromBody] CreateProduct command)
        { 
            var result = await _mediator.Send(command);
            if (!result) return BadRequest("Could not create product.");
            return Ok("Create product successfully.");
            //try
            //{
            //    var result = await _mediator.Send(command);
            //    return Ok(result);
            //}
            //catch (ValidationException ex) // Detect errors
            //{
            //    return BadRequest(ex.Errors.Select(x => x.ErrorMessage));
            //}
            //catch (Exception ex) // 
            //{
            //    return StatusCode(500, "Internal Server Error");
            //}
        }
        [HttpPost("uploadImages-product")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImages(int productId, [FromForm] AddProductImage request)
        {
            request.ProductId = productId;
            var result = await _mediator.Send(request);
            if (!result) return BadRequest("Could not upload images for this product.");
            return Ok("Images uploaded successfully.");
        }
        [HttpGet("get-product-by-id/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var query = new GetProductById { Id = productId };
            var result = await _mediator.Send(query);
            if ( result == null) return NotFound($"Could not find product with {productId}.");
            return Ok(result);
        }
    }
}
    