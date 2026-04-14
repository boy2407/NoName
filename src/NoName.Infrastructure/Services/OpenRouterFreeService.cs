using NoName.Application.Contracts;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace NoName.Application.Services
{
    public class OpenRouterFreeService : IAIService
    {
        private readonly ChatClient _chatClient;
        private readonly string _modelId = "stepfun/step-3.5-flash:free";

        public OpenRouterFreeService(string apiKey)
        {
            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            };

            _chatClient = new ChatClient(_modelId, new ApiKeyCredential(apiKey), options);
        }

        public async Task<string> GetAIResponseAsync(string prompt, string userRole = "Guest", CancellationToken cancellationToken = default)
        {
            try
            {
                var options = new ChatCompletionOptions
                {
                    Temperature = 0.1f,
                    MaxOutputTokenCount = 4000
                };

                List<ChatMessage> messages =
                [
                    new UserChatMessage(prompt)
                ];

                ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, options, cancellationToken);

                return completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                return $"[Error]: {ex.Message}";
            }
        }
    }
}