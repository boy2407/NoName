using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Features.Product.Commands.Create;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProduct command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
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
