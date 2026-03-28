using MediatR;
using System.Security.Claims; 

namespace NoName.Application.Features.Chatbot.Commands
{
    public record AskChatbotCommand(string Message, ClaimsPrincipal? User = null) : IRequest<string>;
}


