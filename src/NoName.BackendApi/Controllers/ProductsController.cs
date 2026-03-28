using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Application.Features.Products.Commands.Create;
using NoName.Application.Features.Products.Commands.Delete;
using NoName.Application.Features.Products.Commands.Options;
using NoName.Application.Features.Products.Commands.Update.common;
using NoName.Application.Features.Products.Commands.Update.Variants;
using NoName.Application.Features.Products.Queries.GetProductsById;
using NoName.Application.Features.Products.Queries.GetProductsPaging;

namespace NoName.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;
        private readonly ILanguageService _languageService;
        public ProductsController(IMediator mediator, ICacheService cacheService, ILanguageService languageService)
        {
            _mediator = mediator;
            _cacheService = cacheService;
            _languageService = languageService;
        }

        //-----------------------COMMANDS----------------------
        [Authorize(policy: "ManagementContent")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var productId = await _mediator.Send(command);
            return Ok(new { Id = productId });
        }


        [Authorize(policy: "ManagementContent")]
        [HttpPost("{id}/variants")]
        public async Task<IActionResult> AddVariant(int id, [FromBody] AddProductVariantCommand command)
        {
            command.ProductId = id;
            var result = await _mediator.Send(command);
            if (!result) return BadRequest("");
            return Ok();
        }

        [Authorize(policy: "ManagementContent")]
        [HttpPost("{id}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImages(int id, [FromForm] AddProductImageCommand command)
        {
            command.ProductId = id;
            var result = await _mediator.Send(command);
            if (!result) return BadRequest("Could not upload images for this product.");
            return Ok("Images uploaded successfully.");
        }

        [Authorize(policy: "ManagementContent")]
        [HttpPost("{id}/options")]
        public async Task<IActionResult> CreateOption(int id, [FromBody] CreateProductOptionCommand command)
        {
            command.ProductId = id;
            var optionId = await _mediator.Send(command);
            return Ok(new { Id = optionId });
        }

        [Authorize(policy: "ManagementContent")]
        [HttpPut("{productId}/options/{optionId}")]
        public async Task<ActionResult<ApiResult<bool>>> UpdateOption(int productId, int optionId, [FromBody] UpdateProductOptionCommand command)
        {
            command.ProductId = productId;
            command.OptionId = optionId;
            return await _mediator.Send(command);
        }

        [Authorize(policy: "ManagementContent")]
        [HttpDelete("{productId}/options/{optionId}")]
        public async Task<ActionResult<ApiResult<bool>>> DeleteOption(int productId, int optionId)
        {
            var command = new DeleteProductOptionCommand { ProductId = productId, OptionId = optionId };
            return await _mediator.Send(command);
        }

        // ----------------------UPDATE----------------------
        [Authorize(policy: "ManagementContent")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<bool>>> Update(int id, [FromBody] UpdateProductCommand command)
        {
            command.Id = id;
            return await _mediator.Send(command);
        }

        [Authorize(policy: "AdminOnly")]
        [HttpPut("{productId}/variants")]
        public async Task<ActionResult<ApiResult<bool>>> UpdateVariants(int productId, [FromBody] List<UpdateVariant> variants)
        {
            var command = new UpdateProductVariantCommand
            {
                ProductId = productId,
                Variants = variants
            };

            return await _mediator.Send(command);
        }



        //------------------------QUERIES----------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            bool isAdmin = User.IsInRole("Admin");
            string role = isAdmin ? "Admin" : "Public";
            var lang = await _languageService.GetCurrentLanguage();
            string cacheKey = CacheKeys.ProductDetail(id, lang, role);

            var cached = await _cacheService.GetAsync<object>(cacheKey);
            if (cached != null) return Ok(cached);

            object result = isAdmin
                ? await _mediator.Send(new AdminGetProductByIdQuery(id))
                : await _mediator.Send(new GetProductByIdQuery(id));
            Console.WriteLine(result);
            if (result != null)
            {
                var cacheTime = isAdmin ? TimeSpan.FromMinutes(10) : TimeSpan.FromHours(1);
                await _cacheService.SetAsync(cacheKey, result, cacheTime);
                return Ok(result);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetProductsPagingQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        // ----------------------DELETE----------------------
        [Authorize(policy: "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
        {
            var command = new DeleteProductCommand { Id = id };
            return await _mediator.Send(command);
        }
    }
}
    