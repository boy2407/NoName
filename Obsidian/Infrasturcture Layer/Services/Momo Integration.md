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
   - `GET /api/payments/momo-callback` → chỉ phục vụ UI (không cập nhật DB), trả thông tin để client chờ kết quả qua SignalR.
   - `POST /api/payments/momo-ipn` → nhận IPN từ MoMo và gọi `UpdatePaymentStatusCommand` để cập nhật DB.
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
  - Lấy Order từ Database
  - Tìm IPaymentService ("MoMo")
  - Gọi paymentService.CreatePaymentAsync(order)
    ↓
    PaymentProviderBase.CreatePaymentAsync()
    ├─ Gọi BuildProviderSpecificUrl() [MomoPaymentService override]
    │  └─ Gọi MoMo API → nhận PayUrl
    ├─ Tạo Transaction entity (Pending)
    ├─ SaveChanges()
    └─ Return PayUrl
  ↓
Controller → Return PayUrl (Redirect client sang Momo)
```

### 2️⃣ **Xác nhận Giao Dịch (Callback IPN)**
```
Momo Redirect → BackendAPI [GET /api/payments/momo-callback?orderId=123&resultCode=0&transactionId=...]
  ↓
PaymentsController.MomoCallback()
  ↓
Controller → Return UI waiting payload (orderId + SignalR hub/event)
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
  - Cập nhật Transaction + Order trong DB
  - Publish SignalR event `PaymentStatusUpdated`
```

### 3️⃣ **Idempotency & Out-of-order Handling**
- Nếu thông báo trùng hoặc đến lệch thứ tự, hệ thống không cho phép downgrade trạng thái từ `Success` xuống `Failed`.
- Mục tiêu: tránh callback/IPN ghi đè trạng thái đã chốt thành công.
- Áp dụng lock theo `orderId` (`lock:payment:order:{orderId}`) bằng Redis RedLock để tránh race condition giữa `callback` và `IPN`.
- Với `resultCode = 1001` (đơn đã được xử lý trước đó), hệ thống coi là duplicate notification và **không cập nhật DB**.
- Khi `Order/Transaction` đã ở trạng thái thành công (`Confirmed/Success`), mọi callback/IPN đến sau chỉ được xử lý idempotent (no-op), không ghi đè dữ liệu.
- Dù callback chỉ còn vai trò UI, race condition vẫn cần giữ vì MoMo có thể gửi IPN retry/trùng request.

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

