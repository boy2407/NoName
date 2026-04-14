using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Services;
using NoName.Domain.Entities;
using NoName.Infrastructure.Settings;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using NoName.Application.Common;
namespace NoName.Infrastructure.Services
{
    public class MomoPaymentService : PaymentProviderBase
    {
        private readonly MomoSettings _momoSettings;
        public MomoPaymentService(
            ILogger<MomoPaymentService> logger,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            IOptions<MomoSettings> momoSettings)
            : base(logger, unitOfWork, configuration, httpClientFactory)
        {
            _momoSettings = momoSettings.Value;
        }

        public override string ProviderName => "MoMo";

        public override async Task<bool> ValidateCallback(IDictionary<string, string> queryParams)
        {
            _logger.LogInformation("Đang xác thực callback từ MoMo...");
            return await Task.FromResult(true);
        }

        protected override async Task<MomoResponse> BuildProviderSpecificUrl(Order order)
        {
            var partnerCode = _momoSettings.PartnerCode;
            var accessKey = _momoSettings.AccessKey;
            var secretKey = _momoSettings.SecretKey;
            var endpoint = _momoSettings.Endpoint;

  
            string orderId = order.Id.ToString() + "NoName"+ Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            long amount = (long)order.TotalAmount;
            long requestTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string orderInfo = $"Thanh toán đơn hàng {orderId}";

            string redirectUrl = _momoSettings.ReturnUrl;
            string ipnUrl = _momoSettings.IpnUrl;

            string requestType = "linkWallet";//captureWallet
            string extraData = ""; 

            _logger.LogInformation("MoMo creating signature for Order: {OrderId}, RequestTime: {RequestTime}", orderId, requestTime);  
            string signature = MakeSignature(accessKey, partnerCode, secretKey, amount, orderId, requestId, orderInfo, redirectUrl, ipnUrl, requestType, extraData, requestTime);

            var payload = new
            {
                partnerCode,
                requestId,
                amount,
                orderId,
                orderInfo,
                redirectUrl,
                ipnUrl,
                requestType,
                extraData,
                signature,
                lang = "vi",
                requestTime  
            };


            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync(endpoint, payload);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<MomoResponse>();

                    if (result != null && result.ResultCode == 0) 
                    {
                        return result;
                    }

                    _logger.LogError($"MoMo trả về lỗi: {result?.Message}");
                }
                else
                {

                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"MoMo API returned {response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi kết nối API MoMo");
            }

            throw new Exception("Không thể tạo liên kết thanh toán MoMo.");
        }

        private string MakeSignature(string accessKey, string partnerCode, string secretKey, long amount, string orderId, string requestId, string orderInfo, string redirectUrl, string ipnUrl, string requestType, string extraData, long requestTime)
        {
     
            var rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={redirectUrl}&requestId={requestId}&requestType={requestType}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

 

}