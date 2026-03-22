using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Features.Categories.Commands.CreateCategory;
using NoName.Application.Features.Categories.Commands.DeleteCategory;
using NoName.Application.Features.Categories.Commands.UpdateCategory;
using NoName.Application.Features.Categories.Queries.GetCategoriesByParentId;
using NoName.Application.Features.Categories.Queries.GetAllCategory;
using NoName.Application.Features.Categories.Queries.GetCategoryByIdWithTranslate;
using NoName.Application.Features.Languages.Queries.GetLanguage;
using System.Windows.Input;

namespace NoName.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator) => _mediator = mediator;

        [Authorize(policy: "ManagementContent")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Authorize(policy: "ManagementContent")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();
        }

        [Authorize(policy: "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            return result ? NoContent() : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesQuery request)
        {

            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}/translations")]
        public async Task<IActionResult> GetCategoryByIdWithTranslates(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdWithTranslatesQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("parents")]
        public async Task<IActionResult> GetByParentId([FromQuery]GetCategoriesByParentIdQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        
    }
}
