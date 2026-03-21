using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Features.Languages.Commands.CreateLanguage;
using NoName.Application.Features.Languages.Commands.DeleteLanguage;
using NoName.Application.Features.Languages.Commands.UpdateLanguage;
using NoName.Application.Features.Languages.Queries.GetLanguage;
using NoName.Application.Features.Languages.Queries.GetLanguageById;

namespace NoName.BackendApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
    public class LanguagesController : Controller
    {
        private readonly IMediator _mediator;

        public LanguagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(policy: "ManagementContent")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLanguageCommand command)
        {
            var result = await _mediator.Send(command);
 
            return Ok(result);
        }
        [Authorize(policy: "ManagementContent")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateLanguageCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Authorize(policy: "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteLanguageCommand(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetLanguagesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {

            var result = await _mediator.Send(new GetLanguageByIdQuery(id));
            return Ok(result);
        }

    }
}
