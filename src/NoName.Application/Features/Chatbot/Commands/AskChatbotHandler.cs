using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using NoName.Application.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace NoName.Application.Features.Chatbot.Commands
{
    public class AskChatbotHandler : IRequestHandler<AskChatbotCommand, string>
    {
        private readonly IAIService _skService;
        private readonly IAuthorizationService _authorizationService;

        public AskChatbotHandler(
            [FromKeyedServices("SemanticKernel")] IAIService skService,
            IAuthorizationService authorizationService)
        {
            _skService = skService;
            _authorizationService = authorizationService;
        }

        public async Task<string> Handle(AskChatbotCommand request, CancellationToken ct)
        {
            var user = request.User;
            var isAuthenticated = user?.Identity?.IsAuthenticated == true;

            if (!isAuthenticated)
            {
                var guestPrompt = $"""
                    Bạn là trợ lý tư vấn sản phẩm cho khách vãng lai.
                    Chỉ tư vấn sản phẩm công khai, không truy cập dữ liệu quản trị.
                    Quy tắc tool:
                    - Khi cần tra danh sách sản phẩm: dùng tool search_fashion_products.
                    - Khi khách hỏi giá theo biến thể cụ thể: dùng tool get_variant_price.
                    - Khi khách hỏi tồn kho: dùng tool get_stock_quantity.
                    - Không tự bịa giá hoặc tồn kho khi chưa gọi tool.

                    User Request: {request.Message}
                    """;

                try
                {
                    return await _skService.GetAIResponseAsync(guestPrompt, "Guest", ct);
                }
                catch (Exception)
                {
                    return "Dịch vụ AI đang tạm thời gián đoạn. Bạn vui lòng thử lại sau nhé.";
                }
            }

            var isVerified = await _authorizationService.AuthorizeAsync(user!, null, "VerifiedUser");
            if (!isVerified.Succeeded)
            {
                return "Tài khoản chưa xác thực email. Vui lòng xác thực email để sử dụng trợ lý AI.";
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(user!, null, "AdminOnly");
            var isManagement = await _authorizationService.AuthorizeAsync(user!, null, "ManagementContent");

            var role = "Customer";
            string systemContext;

            if (isAdmin.Succeeded)
            {
                role = "Admin";
                systemContext = "Bạn là Trợ lý Quản trị viên. Hãy trả lời ngắn gọn, tập trung vào số liệu báo cáo và vận hành.";
            }
            else if (isManagement.Succeeded)
            {
                role = "Staff";
                systemContext = "Bạn là Trợ lý nhân viên vận hành. Hãy hỗ trợ nội dung quản lý ở mức nhân viên/manager.";
            }
            else
            {
                systemContext = "Bạn là nhân viên tư vấn thời trang nhiệt tình. Hãy giúp khách hàng chọn đồ phù hợp.";
            }

            var fullPrompt = $"""
                {systemContext}
                Quy tắc tool:
                - Luôn ưu tiên lấy dữ liệu thật bằng tool trước khi trả lời.
                - Tra sản phẩm: search_fashion_products.
                - Giá theo biến thể: get_variant_price.
                - Tồn kho: get_stock_quantity.
                - Nếu là Admin và người dùng hỏi doanh thu/đơn hàng theo ngày: dùng get_revenue_by_date hoặc get_order_count_by_date.
                - Không suy đoán dữ liệu tài chính, giá, tồn kho.

                User Request: {request.Message}
                """;

            try
            {
                return await _skService.GetAIResponseAsync(fullPrompt, role, ct);
            }
            catch (Exception)
            {
                // Log lỗi tại đây nếu cần (dùng ILogger)
                return "Xin lỗi, hệ thống đang bận. Bạn vui lòng thử lại sau nhé.";
            }
        }
    }
}

// Trong ChatbotController
