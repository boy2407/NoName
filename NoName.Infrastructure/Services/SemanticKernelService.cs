using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using NoName.Application.Contracts;
using NoName.Application.Abstractions.Persistence;
using NoName.Infrastructure.AIPlugins;
using System;
using System.Threading;
using System.Threading.Tasks;
using NoName.Application.Abstractions;

namespace NoName.Infrastructure.Services
{

    public class SemanticKernelService : IAIService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _modelId = "stepfun/step-3.5-flash:free"; 

        // Constructor chỉ inject dependency, KHÔNG Build Kernel ngay
        public SemanticKernelService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        // Method này sẽ dựng Kernel mới cho mỗi Request (Scoped)
        // để đảm bảo Plugin được nạp đúng theo Role
        public async Task<string> GetAIResponseAsync(string prompt, string userRole, CancellationToken ct = default)
        {
            var builder = Kernel.CreateBuilder();
            
  
            var apiKey = _configuration["AI:OpenRouter:ApiKey"]?.Trim();
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return "Cấu hình AI chưa hợp lệ. Vui lòng kiểm tra OpenRouter ApiKey.";
            }

            builder.AddOpenAIChatCompletion(
                modelId: _modelId,
                apiKey: apiKey,
                endpoint: new Uri("https://openrouter.ai/api/v1")
            );

    
            builder.Plugins.AddFromObject(new ProductPlugin(_unitOfWork), "ProductPlugin");

            if (userRole == "Admin") 
            {
                builder.Plugins.AddFromObject(new AdminPlugin(_unitOfWork), "AdminPlugin");
            }


            if (userRole == "Admin" || userRole == "Staff")
            {
                // builder.Plugins.AddFromObject(new StaffPlugin(_unitOfWork), "StaffPlugin");
            }

            var kernel = builder.Build();

            var settings = new OpenAIPromptExecutionSettings
            {
                Temperature = 0.1,
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

     
            var systemPrompt = $"User Role: {userRole}. ";
            if (userRole == "Admin") systemPrompt += "Bạn có quyền truy cập dữ liệu nhạy cảm.";

            try
            {
                return (await kernel.InvokePromptAsync(systemPrompt + prompt, new KernelArguments(settings), cancellationToken: ct)).ToString();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Status: 401", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase))
                {
                    return "Dịch vụ AI đang lỗi xác thực (401). Vui lòng kiểm tra OpenRouter ApiKey.";
                }

                return "Dịch vụ AI tạm thời không khả dụng. Vui lòng thử lại sau.";
            }
        }
    }
}