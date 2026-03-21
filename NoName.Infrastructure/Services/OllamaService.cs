using NoName.Application.Contracts;
using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace NoName.Application.Services
{
    public class OllamaService :IAIService
    {
        private readonly IOllamaApiClient _ollamaClient;

        public OllamaService()
        {
            _ollamaClient = new OllamaApiClient("http://localhost:11434");
            _ollamaClient.SelectedModel = "qwen3:4b";
        }

        public async Task<string> GetAIResponseAsync(string prompt, CancellationToken ct = default)
        {

            var request = new GenerateRequest
            {
                Prompt = prompt,
                Options = new RequestOptions
                {
                    Temperature = 0f, 
                    TopP = 0.1f       
                }
            };

            var response = "";

            // 2. Truyền request object thay vì string prompt
            await foreach (var stream in _ollamaClient.GenerateAsync(request, ct))
            {
                if (stream != null)
                {
                    response += stream.Response;
                }
            }

            return response;
        }
    }
}
