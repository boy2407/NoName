using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Features.Product.Commands.Create;
using NoName.Application.Features.Product.DTOs;
using NoName.Application.Features.Product.Queries.GetProductsPaging;

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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProduct command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);

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

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging([FromQuery] GetProductPaging query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Test get Product");
        }
    }
}
