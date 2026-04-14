# Cấu hình EF Core (DbContext & Mapping)

- **Vị trí**: `/EF` thuộc `[[Infrastructure Layer]]`.
- **Vai trò**: Đại điện kết nối ORM với hệ quản trị CSDL trung tâm (Tương thích lớn nhất với SQL Server/PostgreSQL).
- **Quy tắc thiết kế (`copilot-instructions`)**:
  - Không bao giờ đặt `[Table("Orders")]` hoặc `[Key]` tại `[[Domain Layer]]`. Toàn bộ sẽ được thiết lập bằng **Fluent API** ở đây bằng cách override `OnModelCreating(ModelBuilder modelBuilder)`.
  - Tách nhỏ từng file cho logic mapping: Các cấu hình riêng nên ở thư mục `Configurations/` (`OrderConfiguration : IEntityTypeConfiguration<Order>`).
  - Gắn logic `Interceptors` (Ví dụ: tự động điền giá trị thuộc tính `CreatedDate` và `UpdatedDate` mỗi khi đối tượng Save).
  - Tích hợp Identity (ví dụ thừa kế `IdentityDbContext<User, Role, Guid>`).