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
        public async Task<IActionResult> MomoCallback()
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

                var command = new UpdatePaymentStatusCommand(callbackData, "MoMo");
                var updateResult = await mediator.Send(command);

                if (!updateResult.Processed)
                {
                    return BadRequest(ApiResult<string>.Failure(updateResult.Message ?? "Callback processing failed"));
                }

                var uiMessage = resultCode == "0"
                    ? $"Thanh toán đơn {orderId} đã được cập nhật từ callback. Hệ thống vẫn nhận IPN để đối soát idempotent."
                    : $"Đã xử lý callback cho đơn {orderId} (resultCode: {resultCode}). Nếu IPN đến sau sẽ tự bỏ qua khi trạng thái đã chốt.";

                return Ok(ApiResult<object>.Success(new
                {
                    orderId,
                    resultCode,
                    processed = updateResult.Processed,
                    isSuccess = updateResult.IsSuccess,
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
