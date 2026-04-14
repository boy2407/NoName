# Application Layer

Là khu vực **trái tim** chứa Workflow (Luồng công việc) của hệ thống. Đây là nơi các Use Cases của ứng dụng NoName được định nghĩa. Nó kết nối `[[Domain Layer]]` với các `[[Infrastructure Layer]]` (được ẩn sau Abstractions).

## Các thành phần chính và thư mục tương ứng:

1. **Giao tiếp Hệ Thống (`/Abstractions`)**
   - Đặt Interfaces (Ví dụ: `IOrderRepository`, `ICartRepository` ở `/Persistence`, và `IPaymentService`, `ICacheService` ở `/Services`). Tầng Hạ tầng sẽ phải thực hiện nó (`[[Persistence]]` và `[[Services]]`).
   - Đảm bảo tuân thủ tính _Inversion of Control - IoC_.

2. **Features (Cơ chế Nhóm Use Case với `/Features`)**
   - Ứng dụng mô hình **CQRS** (Command Query Responsibility Segregation) + **MediatR**.
   - Phải chia rõ ràng thư mục thành: `Commands` & `Queries`.
   - **Tên class bắt buộc kết thúc bằng `Command` hoặc `Query`**. Handler bắt buộc phải khớp. (Ví dụ: `CreateOrderCommand`, `CreateOrderCommandHandler`).
   - Handler tuyệt đối **KHÔNG CÓ TÌNH TRẠNG "FAT HANDLER"** cho caching xử lý - đẩy tác vụ caching vào class Service.

3. **Xác thực dữ liệu (Validation)**
   - Sử dụng `FluentValidation` để thẩm định tất cả Request từ Client.
   - Thường nằm chung với Commands (Ví dụ `CreateOrderCommandValidator`).

4. **Biến Đổi Data (`/Mapping`)**
   - Model (DTO/View Model) sang Entity (Ví dụ `Mapster` hoặc `AutoMapper`). Tránh trả thẳng Entity ra ngoài Controller.

5. **Dịch Vụ Cụ Thể (`/Services`)**
   - Abstract base service như `PaymentProviderBase` hoặc logic kết tinh cần dùng lại qua các Handler.

## Workflow Căn Bản
1. API Controller gửi một `{Model}Command` -> `MediatR` bắt lấy.
2. `Pipeline Behavior` chạy Validation (qua `FluentValidation`). Ném lỗi `ValidationException` nếu sai.
3. `XHandler` nhận request -> Gọi Interface (Vào Database/Redis) -> Lưu thay đổi bằng UnitOfWork `.SaveChangesAsync()`.
4. Trả kết quả (Result/DTO).