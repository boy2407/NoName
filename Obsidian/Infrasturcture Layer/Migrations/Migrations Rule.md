# Quản lý Database Migrations

- **Vị trí**: `/Migrations` thuộc `[[Infrastructure Layer]]`.
- **Vai trò**: Cung cấp phiên bản cập nhật tự động (Versioning) cho CSDL (`NoNameDbContext`) của hệ thống dựa trên thay đổi Entity FrameWork.
- **Quy tắc thiết kế (`copilot-instructions`)**:
  - Khi thay đổi Cấu trúc (Property `Order`) của `[[Domain Layer]]` và cấu hình lại tại `[[EF Core]]`, bắt buộc phải tạo Migration thông qua công cụ dòng lệnh (dotnet ef migrations add ...).
  - Không xoá hay sửa lại Entity Database thủ công bằng tay (SQL Management Studio).
  - Kiểm tra script Up() và Down() ngay sau khi Generate xem có bị DROP Table không mong muốn.
  - Quá trình deploy sẽ tự Update hoặc sử dụng CI/CD qua `NoName.BackendApi`.