using MediatR;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Features.Chatbot.Commands;

namespace NoName.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IMediator _mediator;


        public AiController(IMediator mediator)
        {
            _mediator = mediator;
        }

 
        [HttpGet("ask")]
        public async Task<IActionResult> Ask([FromQuery] AskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest("Question cannot be empty.");
            }

            var result = await _mediator.Send(new AskChatbotCommand(request.Question, User));

      
            return Ok(new { Answer = result });
        }
    }


    public class AskRequest
    {
        public string Question { get; set; }
    }
}