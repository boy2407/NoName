# Presentation Layer

Đại diện cho tầng Ngoài Lề (Outer Edge), chính là nơi mà thế giới giao tiếp với ứng dụng bằng các API Request, Tín hiệu giao diện, SignalR (WebSockets)... 

## Các thành phần chính và thư mục tương ứng:

1. **Ứng Dụng Khách Backend Khách (Client)**: **Blazor WebAssembly (`NoName.AdminApp` / `NoName.Shared`)**
   - Không chứa CSDL. Mọi request phải đi qua BackendApi.
   - Thư mục `Shared` thường là nơi trao đổi DTO giữa Frontend (Blazor Wasm) và Backend (Api).
   - Sử dụng HttpClientFactory để gọi API với Token Bearer Authorization.

2. **Dịch Vụ Cung Cấp Tính Năng (WebAPI `NoName.BackendApi`)**
   - Điểm truy cập của hệ thống (Entry Point). Nơi nhận HTTP Methods (GET, POST, PUT, DELETE).
   - Setup Minimal API hoặc API Controllers tuỳ tiêu chuẩn.

## Quy tắc thiết kế (`copilot-instructions`)
- **Controller Mỏng**: Không có code xử lý (Business Logic) trong `Controller/Endpoint`. Controller duy nhất làm chức năng:
  - Map Route/Path từ Request.
  - Wrap Request DTO thành Object theo class của `CQRS` (`[X]Command` hoặc `[Y]Query`).
  - Gửi nó qua Mediator (`await _mediator.Send(command)`).
  - Trả về HTTP Status Code (200, 400, 404, 201) tương ứng với Result từ Application Layer.
- Khởi động môi trường và kết nối **[[Dependency Injection]]** nằm tại `Program.cs`. Mặc định DI Register đã được định nghĩa tại các dự án dưới dạng `IServiceCollection Extension Methods` và được gọi về đây: (ví dụ: `builder.Services.AddApplication()`, `.AddInfrastructure()`).