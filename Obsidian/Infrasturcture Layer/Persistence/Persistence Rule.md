# Quản lý Persistence (Repositories)

- **Vị trí**: `/Persistence` thuộc `[[Infrastructure Layer]]`.
- **Vai trò**: Cung cấp triển khai mã của các Interfaces `IRepository` từ tầng `[[Application Layer]]` bằng `DbContext` của `[[EF Core Rule]]`.
- **Quy tắc thiết kế (`copilot-instructions`)**:
  - Không tự gõ thêm phương thức public không được khai báo bên Abstraction.
  - Sử dụng C# 12 Primary Constructors `public class OrderRepository(NoNameDbContext dbContext) : IOrderRepository` thay vì cách `readonly` truyền thống.
  - Tracking State (Trạng thái thay đổi): **KHÔNG BAO GIỜ** gọi `SaveChangeAsync()` bên trong `OrderRepository`. Bạn chỉ cần Update/Add/Remove Entity Tracking trong DBContext ở đây, và thực hiện Commit cuối cùng qua `[[Unit of Work]]` trong CQRS Handler (Tầng Application).
  - Tối ưu truy vấn (Đặc biệt với Command Queries read-only): Có thể sử dụng `.AsNoTracking()` trong Entity Framework truy vấn tốc độ cao (GetList).
  - Nếu Logic tính Toán như Doanh thu (Ví dụ `GetRevenueByMonthAsync`), tách ra các Service độc lập hơn hoặc dùng Raw SQL/Dapper cho tốc độ.