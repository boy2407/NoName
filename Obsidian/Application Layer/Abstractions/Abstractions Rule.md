# Quản lý Abstractions (Interfaces)

- **Vị trí**: `/Abstractions` thuộc `[[Application Layer]]`.
- **Vai trò**: Chứa các Interface định nghĩa hợp đồng (Contracts) giao tiếp với các thư viện hoặc công nghệ bên ngoài. (Ví dụ: `IOrderRepository`, `ICartRepository` ở `/Persistence` và `IPaymentService`, `ICacheService` ở `/Services`).
- **Quy tắc thiết kế**:
  - Đảm bảo tính Inversion of Control (IoC). `[[Application Layer]]` gọi Interface, và Hạ tầng (`[[Infrastructure Layer]]`) là nơi phải tuân thủ và viết code thực thi (Implementation).
  - Không có logic tại đây. Chỉ chứa khái niệm (Signature).