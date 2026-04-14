# Quản lý Features (CQRS + MediatR)

- **Vị trí**: `/Features` thuộc `[[Application Layer]]`.
- **Vai trò**: Đại diện cho "Workflow" của ứng dụng NoName. Đây là cốt lõi của nguyên tắc Clean Architecture.
- **Quy tắc thiết kế**:
  - Áp dụng CQRS (Command Query Responsibility Segregation) + MediatR.
  - Tách thư mục rõ ràng giữa `Commands` (thay đổi trạng thái - POST/PUT/DELETE) và `Queries` (Đọc dữ liệu - GET).
  - Tên File bắt buộc phải kết thúc bằng `Command` hoặc `Query`. Handler phải khớp (Ví dụ: `CreateOrderCommand`, `CreateOrderCommandHandler`).
  - Validation: Tách `FluentValidation` validator vào cùng cấp với Command, và Pipeline của MediatR sẽ chặn lỗi trả về nếu không hợp lệ.
  - Tránh Fat Handlers: Tuyệt đối không nhét logic Cache trực tiếp tại Handler. (Sử dụng `[[Concurrency Management]]` hoặc `ICacheService`).