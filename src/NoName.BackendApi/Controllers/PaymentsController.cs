using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Common;
using NoName.Application.Features.Payments.Commands.CreatePayment;
using NoName.Application.Features.Payments.Commands.UpdatePaymentStatus;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoName.BackendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController(IMediator mediator) : ControllerBase
    {
        [HttpPost("create/{orderId}")]
        public async Task<IActionResult> CreatePayment(int orderId, [FromQuery] string provider)
        {
            try
            {
                var command = new CreatePaymentCommand(orderId, provider);
                var payUrl = await mediator.Send(command);
                return Ok(ApiResult<string>.Success(payUrl));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<string>.Failure(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet("momo-callback")]
        public IActionResult MomoCallback()
        {
            try
            {
                var callbackData = Request.Query
                    .ToDictionary(x => x.Key, x => x.Value.ToString());

                var orderId = callbackData.TryGetValue("orderId", out var oid) ? oid : string.Empty;
                var resultCode = callbackData.TryGetValue("resultCode", out var rc) ? rc : string.Empty;

                if (string.IsNullOrWhiteSpace(orderId))
                {
                    return BadRequest(ApiResult<string>.Failure("Missing orderId in callback"));
                }

                var uiMessage = resultCode == "0"
                    ? $"Đang xác nhận thanh toán cho đơn {orderId}. Vui lòng chờ IPN cập nhật trạng thái."
                    : $"Đã nhận callback cho đơn {orderId} (resultCode: {resultCode}). Đang chờ IPN xác thực cuối cùng.";

                return Ok(ApiResult<object>.Success(new
                {
                    orderId,
                    resultCode,
                    waitingForIpn = true,
                    signalRHub = "/hubs/payment-status",
                    signalREvent = "PaymentStatusUpdated"
                }, uiMessage));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<string>.Failure($"Payment processing error: {ex.Message}"));
            }
        }

        [AllowAnonymous]
        [HttpPost("momo-ipn")]
        public async Task<IActionResult> MomoIpn([FromBody] Dictionary<string, object> payload)
        {
            try
            {
                var callbackData = (payload ?? new Dictionary<string, object>())
                    .ToDictionary(x => x.Key, x => x.Value?.ToString() ?? string.Empty);

                var command = new UpdatePaymentStatusCommand(callbackData, "MoMo");
                var updateResult = await mediator.Send(command);

                if (!updateResult.Processed)
                {
                    return BadRequest(ApiResult<string>.Failure(updateResult.Message ?? "IPN processing failed"));
                }

                return Ok(new { message = "IPN received" });
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResult<string>.Failure($"IPN processing error: {ex.Message}"));
            }
        }
    }
}
