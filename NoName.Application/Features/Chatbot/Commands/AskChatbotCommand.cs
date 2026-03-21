using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Chatbot.Commands
{
    public record AskChatbotCommand(string Message) : IRequest<string>;
}
