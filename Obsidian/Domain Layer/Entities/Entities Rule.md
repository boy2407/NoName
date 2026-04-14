# Quản lý Entities

- **Vị trí**: `/Entities` thuộc `[[Domain Layer]]`.
- **Vai trò**: Đại diện cho các đối tượng cốt lõi trong hệ thống (Ví dụ: `Order`, `User`, `Transaction`).
- **Quy tắc thiết kế**:
  - Không sử dụng Data Annotations của Entity Framework (như `[Table]`, `[Key]`) tại đây. Toàn bộ cấu hình CSDL phải dùng Fluent API ở `[[EF Core]]` (tầng Infrastructure).
  - Ưu tiên tính Đóng gói (Encapsulation). Dùng `private set` cho các Property có Logic thay đổi phức tạp và cung cấp các public method để thay đổi trạng thái (Ví dụ: `public void UpdateStatus(...)`).