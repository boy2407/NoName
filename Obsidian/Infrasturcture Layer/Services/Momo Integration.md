# Momo Integration (Tích hợp thanh toán Momo)

## 📌 Tổng quan
Tính năng tích hợp Momo cung cấp giải pháp thanh toán qua ví điện tử. Tuân thủ **Clean Architecture** và **CQRS Pattern**, phần tích hợp với bên thứ 3 (External Service) này được thiết kế với quy trình Command-Query rõ ràng.

## 📂 Vị trí và Kiến trúc dự án
1. **Application Layer - Features/Payments**:
   - **Commands**:
     - `CreatePaymentCommand` + `CreatePaymentCommandHandler`: Xử lý tạo yêu cầu thanh toán.
      - `UpdatePaymentStatusCommand` + `UpdatePaymentStatusCommandHandler`: Xử lý callback từ Momo, cập nhật trạng thái Order và Transaction.
      - `UpdatePaymentStatusResult`: tách rõ kết quả xử lý kỹ thuật (`Processed`) và kết quả thanh toán (`IsSuccess`).
   - **Validators**: `CreatePaymentValidator`, `UpdatePaymentStatusValidator` sử dụng FluentValidation.
   - **Abstractions**: 
     - `IPaymentService` interface định nghĩa dịch vụ thanh toán chung.
   - **Services Base**:
     - `PaymentProviderBase` abstract class cung cấp Template Method Pattern, chứa logic tạo Transaction chung cho tất cả providers.

2. **Infrastructure Layer - Services**:
   - `MomoPaymentService`: Triển khai cụ thể cho Momo, kế thừa `PaymentProviderBase`.
     - Overrides `ProviderName` (= "MoMo").
     - Overrides `BuildProviderSpecificUrl`: Gọi MoMo API để tạo Payment URL.
     - Overrides `ValidateCallback`: Tạm thời luôn trả về `true` theo yêu cầu hiện tại (TODO: thêm signature validation).

3. **Presentation Layer (BackendApi - PaymentsController)**:
   - **Thin Controller Pattern**: Controller chỉ điều hướng MediatR Commands/Queries, KHÔNG chứa logic.
   - `POST /api/payments/create/{orderId}` → gọi `CreatePaymentCommand`.
   - `GET /api/payments/momo-callback` → gọi `UpdatePaymentStatusCommand` để cập nhật DB ngay khi user redirect về hệ thống, đồng thời trả payload SignalR cho UI.
   - `POST /api/payments/momo-ipn` → nhận IPN từ MoMo và gọi lại `UpdatePaymentStatusCommand` theo cơ chế idempotent.
   - `SignalR Hub /hubs/payment-status` → đẩy event `PaymentStatusUpdated` sau khi IPN xử lý.

## 🛠 Flow Hệ Thống (CQRS + Strategy Pattern)

### 1️⃣ **Tạo Thanh Toán (CreatePaymentCommand)**
```
Client → BackendAPI [POST /api/payments/create/123?provider=MoMo]
  ↓
PaymentsController.CreatePayment()
  ↓
MediatR → CreatePaymentCommandHandler
  ↓
  ** DUPLICATE PREVENTION & EXPIRATION CHECK START **
  - Kiểm tra xem Order đã có Transaction (Pending hoặc Success) chưa
  - Nếu có Transaction pending + PayUrl hợp lệ (< 25 phút) → Trả về PayUrl cũ (từ DB)
  - Nếu có Transaction pending + PayUrl hết hạn (> 25 phút):
      * Mark old transaction as Failed
      * Tạo payment mới (để tránh MoMo "transaction expired" error)
  - Nếu có Transaction success → Reject với error "Already paid"
  ** DUPLICATE PREVENTION & EXPIRATION CHECK END **
  ↓
  - Tìm IPaymentService ("MoMo")
  - Gọi paymentService.CreatePaymentAsync(order)
    ↓
    PaymentProviderBase.CreatePaymentAsync()
    ├─ Gọi BuildProviderSpecificUrl() [MomoPaymentService override]
    │  └─ MomoPaymentService:
    │     * orderId = order.Id (consistent - no GUID)
    │     * requestTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
    │     * signature includes requestTime parameter
    │     * Gọi MoMo API với payload + requestTime → nhận PayUrl
    ├─ Tạo Transaction entity (Pending) + lưu PayUrl
    ├─ SaveChanges()
    └─ Return PayUrl
  ↓
Controller → Return PayUrl (Redirect client sang Momo)
```

**Lợi ích Duplicate Prevention & Expiration Handling:**
- ✅ Nếu client retry do network error (< 25 phút) → không tạo duplicate payment
- ✅ Nếu client retry sau hết hạn (> 25 phút) → tạo payment mới, tránh MoMo "transaction expired"
- ✅ MoMo API không từ chối vì duplicate orderId (thanks to consistent orderId)
- ✅ MoMo không từ chối vì expired payment link (thanks to expiration check + new link)
- ✅ Cùng lúc chỉ 1 Transaction pending cho mỗi Order
- ✅ Có thể an toàn retry khi connection timeout

### 2️⃣ **Xác nhận Giao Dịch (Callback IPN)**
```
Momo Redirect → BackendAPI [GET /api/payments/momo-callback?orderId=123&resultCode=0&transactionId=...]
  ↓
PaymentsController.MomoCallback()
  ↓
MediatR → UpdatePaymentStatusCommandHandler
  ↓
  - Cập nhật Transaction + Order trong DB ngay tại callback
  ↓
Controller → Return UI payload (orderId + SignalR hub/event)
  ↓
Client connect SignalR + JoinOrderGroup(orderId)
```

`IpnUrl` từ MoMo sẽ gọi:
```
Momo IPN → BackendAPI [POST /api/payments/momo-ipn]
  ↓
PaymentsController.MomoIpn()
  ↓
MediatR → UpdatePaymentStatusCommandHandler
  ↓
  - Cập nhật Transaction + Order trong DB (hoặc no-op nếu callback đã xử lý trước)
  - Publish SignalR event `PaymentStatusUpdated`
```

### 3️⃣ **Idempotency & Out-of-order Handling**
- Nếu thông báo trùng hoặc đến lệch thứ tự, hệ thống không cho phép downgrade trạng thái từ `Success` xuống `Failed`.
- Mục tiêu: tránh callback/IPN ghi đè trạng thái đã chốt thành công.
- Áp dụng lock theo `orderId` (`lock:payment:order:{orderId}`) bằng Redis RedLock để tránh race condition giữa `callback` và `IPN`.
- Với `resultCode = 1001` (đơn đã được xử lý trước đó), hệ thống coi là duplicate notification và **không cập nhật DB**.
- Khi `Order/Transaction` đã ở trạng thái thành công (`Confirmed/Success`), mọi callback/IPN đến sau chỉ được xử lý idempotent (no-op), không ghi đè dữ liệu.
- Dù callback chỉ còn vai trò UI, race condition vẫn cần giữ vì MoMo có thể gửi IPN retry/trùng request.


## ⚠️ Troubleshooting: Common MoMo Errors

### **"Giao dịch đã hết hạn" (Transaction Expired)**
**Nguyên nhân:**
1. **PayUrl cũ được tái sử dụng sau > 30-60 phút**: MoMo URLs có thời hạn cố định
   - Solution: Hệ thống kiểm tra age of PayUrl (< 25 phút) trước khi tái sử dụng

2. **Thiếu requestTime trong payload**: MoMo xác thực timestamp
   - Solution: Đã thêm `requestTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()`

3. **requestTime không khớp trong signature**: Signature phải include requestTime
   - Solution: MakeSignature() đã thêm `&requestTime={requestTime}` vào rawHash

**Cách khắc phục:**
```csharp
// CreatePaymentCommandHandler - Tự động tạo payment mới nếu PayUrl hết hạn
if (ageInMinutes < 25)  // Reuse if fresh
    return existingPayUrl;
else  // Create new payment if expired
    pendingOrSuccessTransaction.Status = TransactionStatus.Failed;
    // Tiếp tục tạo payment mới
```

### **"orderId trùng" (Duplicate OrderId)**
**Nguyên nhân:**
- orderId thay đổi mỗi lần gọi (do GUID suffix)
- MoMo từ chối request với cùng orderId trong thời gian ngắn

**Cách khắc phục:**
```csharp
// MomoPaymentService - Dùng orderId nhất quán
string orderId = order.Id.ToString();  // ✅ Không thêm GUID
// Mỗi order chỉ có 1 orderId, MoMo duplicate detection hoạt động đúng
```

### **"Signature không khớp" (Invalid Signature)**
**Nguyên nhân:**
- Thứ tự parameters trong rawHash sai
- requestTime không được include
- Mã hoá không đúng

**Cách khắc phục:**
```csharp
// MakeSignature() - Thứ tự chính xác + include requestTime
var rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}" +
              $"&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}" +
              $"&partnerCode={partnerCode}&redirectUrl={redirectUrl}" +
              $"&requestId={requestId}&requestTime={requestTime}" +  // ✅ requestTime here
              $"&requestType={requestType}";
```

## 🔒 Quy tắc bảo mật và Configuration

### Configuration
- Thông tin MoMo (`PartnerCode`, `AccessKey`, `SecretKey`, `Endpoint`, `ReturnUrl`, `IpnUrl`) nằm ở `appsettings.json` trong mục `PaymentSettings:Momo`.
- Sử dụng `MomoSettings` class để strongly-typed configuration.

### Security Notes
- ⚠️ **Hiện tại**: Callback validation đang tạm thời **always true**, **chưa validate signature**.
- 🔧 **TODO**: Implement HMAC-SHA256 signature validation để chống giả mạo callback.
- 📝 Mọi giao dịch phải được Log chi tiết để phục vụ đối soát.

## 📋 Thêm Payment Provider Mới (VNPay, ZaloPay)
1. Tạo `VNPayPaymentService : PaymentProviderBase` trong `Infrastructure/Services/`.
2. Override `ProviderName` (= "VNPay").
3. Override `BuildProviderSpecificUrl()` với logic của VNPay API.
4. Override `ValidateCallback()` với validation logic.
5. Thêm vào DependencyInjection: `services.AddScoped<IPaymentService, VNPayPaymentService>();`
6. Tạo `VNPaySettings` trong `appsettings.json`.
7. Tạo tests cho VNPayPaymentService.

## 🔗 Liên kết liên quan
- Entities: [[Order]], [[Transaction]]
- Enums: [[OrderStatus]], [[TransactionStatus]]
- Commands: [[CreatePaymentCommand]], [[UpdatePaymentStatusCommand]]
- Handlers: [[CreatePaymentCommandHandler]], [[UpdatePaymentStatusCommandHandler]]
- Services: [[IPaymentService]], [[MomoPaymentService]], [[PaymentProviderBase]]
- Controller: [[PaymentsController]]
- Config: [[appsettings.json]], [[MomoSettings]]
- Dependency Injection: [[Infrastructure/DependencyInjection]]

## 🎯 Checklist Cần Làm
- [x] Implement HMAC-SHA256 signature validation trong MomoPaymentService.ValidateCallback()
- [ ] Thêm unit tests cho CreatePaymentCommandHandler
- [ ] Thêm unit tests cho UpdatePaymentStatusCommandHandler
- [ ] Implement VNPay payment provider
- [ ] Implement ZaloPay payment provider
- [ ] Thêm retry logic cho failed transactions
- [ ] Dashboard để theo dõi transactions (Admin)

