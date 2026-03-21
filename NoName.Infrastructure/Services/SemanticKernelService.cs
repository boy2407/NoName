using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using NoName.Application.Contracts;
using System.Threading;

namespace NoName.Infrastructure.Services
{
    public class SemanticKernelService : IAIService
    {
        private readonly Kernel _kernel;

        public SemanticKernelService()
        {
            // 1. Cấu hình Builder trỏ về Ollama (Ollama hỗ trợ chuẩn OpenAI ở /v1)
            var builder = Kernel.CreateBuilder();

            // Sử dụng AddOpenAIChatCompletion nhưng trỏ Endpoint về Localhost của Ollama
            builder.AddOpenAIChatCompletion(
                modelId: "qwen3:4b",
                apiKey: "no-key-needed", // Ollama không cần key nhưng thư viện yêu cầu truyền string
                endpoint: new Uri("http://localhost:11434/v1")
            );

            // 2. Đăng ký các Plugin (Tools) của bạn tại đây để AI có thể "thấy" Database
            // builder.Plugins.AddFromObject(new ProductPlugin(...)); 

            _kernel = builder.Build();
        }

        public async Task<string> GetAIResponseAsync(string prompt, CancellationToken ct = default)
        {
            // Chỉ generate text; không bật AutoInvoke tools để tránh lỗi gọi function không cần thiết.
            var settings = new OpenAIPromptExecutionSettings
            {
                Temperature = 0,
                MaxTokens = 1000
            };

            // 4. Chạy Prompt với cấu trúc Arguments để truyền Settings vào
            var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments(settings), cancellationToken: ct);

            return result.ToString();
        }
    }
}