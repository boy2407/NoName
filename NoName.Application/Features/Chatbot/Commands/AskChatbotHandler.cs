using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Contracts;
using NoName.Application.Features.Chatbot.DTOs;
using NoName.Application.Services;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
namespace NoName.Application.Features.Chatbot.Commands
{
    public class AskChatbotHandler : IRequestHandler<AskChatbotCommand, string>
    {
        private readonly IAIService _ollamaService;
        private readonly IAIService _skService;
        private readonly IAIService _openRouterFreeService;
        private readonly IUnitOfWork _unitOfWork;

        public AskChatbotHandler(
            [FromKeyedServices("Ollama")] IAIService ollamaService,
            [FromKeyedServices("SemanticKernel")] IAIService skService,
            [FromKeyedServices("OpenRouter")] IAIService openRouterFreeService,
            IUnitOfWork unitOfWork)
        {
            _ollamaService = ollamaService;
            _skService = skService;
            _openRouterFreeService = openRouterFreeService;
            _unitOfWork = unitOfWork;
        }

        private const string AiDictionary = """
            Vai trò: Bạn là bộ phân tích truy vấn sản phẩm thời trang.
            Mục tiêu: Chuyển câu người dùng thành JSON để lọc dữ liệu.

            OUTPUT BẮT BUỘC:
            - Chỉ trả về đúng 1 JSON object hợp lệ.
            - Không thêm mô tả, không thêm markdown, không thêm ký tự ngoài JSON.
            - Chỉ dùng đúng key và kiểu dữ liệu theo schema.

            SCHEMA:
            {
              "LanguageId": "vi-VN | en-US",
              "Category": "string",
              "Materials": ["string"],
              "Colors": ["string"],
              "Tags": ["string"],
              "MaxPrice": 0
            }

            QUY TẮC GIÁ TRỊ MẶC ĐỊNH:
            - Không xác định được string => ""
            - Không xác định được list => []
            - Không nhắc đến giá => MaxPrice = null

            TỪ ĐIỂN CHO PHÉP:
            LanguageId:
            - "vi-VN" (tiếng Việt)
            - "en-US" (tiếng Anh)

            Category (chỉ 1 giá trị hoặc ""):
            - "Áo thun", "Áo sơ mi", "Áo len", "Áo khoác", "Áo polo", "Áo dài", "Quần jean", "Quần short", "Quần dài"

            Materials:
            - "Cotton hữu cơ", "Sợi tre Bamboo", "Lụa tơ tằm", "Denim nguyên bản", "Len Merino", "Vải Linen"

            Colors:
            - "Đen", "Trắng", "Xanh Navy", "Xám ghi", "Be", "Hồng"

            Tags:
            - Occasion: "Công sở", "Dạo phố", "Dự tiệc", "Thể thao"
            - Style: "Thanh lịch", "Năng động", "Tối giản", "Cá tính"
            - Weather: "Mùa hè", "Mùa đông", "Thu đông", "Mọi thời tiết"
            - Feature: "Thoáng khí", "Co giãn", "Chống nhăn", "Giữ nhiệt"

            CHUẨN HÓA TỪ ĐỒNG NGHĨA:
            - hè, summer => "Mùa hè"
            - đông, winter, lạnh => "Mùa đông"
            - thu đông, autumn => "Thu đông"
            - đi làm, office => "Công sở"
            - đi chơi, casual => "Dạo phố"
            - tiệc, party => "Dự tiệc"
            - gym, sport => "Thể thao"
            - đen, black => "Đen"
            - trắng, white => "Trắng"
            - navy => "Xanh Navy"
            - gray, grey, xám => "Xám ghi"
            - beige => "Be"
            - pink => "Hồng"

            QUY TẮC GIÁ:
            - "500k" => 500000
            - "1 triệu", "1tr" => 1000000
            - "dưới X", "tối đa X", "< X" => MaxPrice = X
            - "3 xị","3 lít" => 300000 
            VÍ DỤ:
            Input: "Tìm áo thun đi làm màu đen dưới 500k"
            Output: {"LanguageId":"vi-VN","Category":"Áo thun","Materials":[],"Colors":["Đen"],"Tags":["Công sở"],"MaxPrice":500000}

            Input: "I need a winter jacket, navy color"
            Output: {"LanguageId":"en-US","Category":"Áo khoác","Materials":[],"Colors":["Xanh Navy"],"Tags":["Mùa đông"],"MaxPrice":null}

            Input: "Mình cần đồ thoáng khí, co giãn để dạo phố"
            Output: {"LanguageId":"vi-VN","Category":"","Materials":[],"Colors":[],"Tags":["Thoáng khí","Co giãn","Dạo phố"],"MaxPrice":null}
            """;

        public async Task<string> Handle(AskChatbotCommand request, CancellationToken ct)
        {
            // Bước 1: Extract JSON - Dùng OllamaService (Nhanh, nhẹ, không bị lỗi AutoInvoke)
            var aiPrompt = $"{AiDictionary}\n\nUser Message: \"{request.Message}\"";
            var jsonResponse = await _openRouterFreeService.GetAIResponseAsync(aiPrompt);

            AiSearchCriteria criteria;
            try
            {
                var cleanJson = jsonResponse.Replace("```json", "").Replace("```", "").Trim();
                criteria = JsonSerializer.Deserialize<AiSearchCriteria>(cleanJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;


                var debugJson = JsonSerializer.Serialize(criteria, new JsonSerializerOptions
                {
                    WriteIndented = true // Giúp JSON in ra có xuống dòng, dễ đọc
                });

                Console.WriteLine("======= AI EXTRACTED CRITERIA =======");
                Console.WriteLine(debugJson);
                Console.WriteLine("=====================================");


            }
            catch
            {
                return "Dạ, em chưa rõ ý mình. Anh/Chị muốn tìm đồ màu gì hoặc chất liệu gì ạ?";
            }

            // Bước 2: Truy vấn Database
            var products = await _unitOfWork.Products.SearchByAiCriteriaAsync(criteria);

            if (!products.Any()) return "Xin lỗi, em không tìm thấy sản phẩm phù hợp.";

            var productInfo = string.Join("\n", products.Take(5).Select(p =>
                $"- {p.ProductTranslations.FirstOrDefault()?.Name} (Giá: {p.ProductVariants.FirstOrDefault()?.Price})"));

            // Bước 3: Gen Response - Dùng Semantic Kernel (Hoặc OllamaService đều được)
            var finalPrompt = $"""
                    QUY TẮC BẮT BUỘC (Strict Rules):
                    1. CHỈ tư vấn dựa trên danh sách sản phẩm trong phần [Context].
                    2. KHÔNG tự tạo ra sản phẩm, thương hiệu hoặc mức giá không có trong [Context].
                    3. Trả lời ngắn gọn, tự nhiên bằng ngôn ngữ: {criteria.LanguageId}
    
                    [Context]
                    {productInfo}
                    [/Context]
    
                    Query: {request.Message}
                    """;

            // Truyền CancellationToken xuống service nếu có thể để handle ngắt kết nối từ client
            try
            {
                return await _openRouterFreeService.GetAIResponseAsync(finalPrompt, ct);
            }
            catch (OperationCanceledException)
            {
                return "Yêu cầu đã bị hủy. Anh/Chị vui lòng thử lại giúp em nhé.";
            }
        }
    }
}