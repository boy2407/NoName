using MediatR;
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetLanguages();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetById-{id}")]
        public async Task<IActionResult> GetById(string id)
        {

            var result = await _mediator.Send(new GetLanguageById(id));
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLanguage command)
        {
            var result = await _mediator.Send(command);
 
            return Ok(result);
        }

        [HttpPut("update-{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateLanguage command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete-{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.Send(new DeleteLanguage(id));
            return Ok(result);
        }
    }
}
