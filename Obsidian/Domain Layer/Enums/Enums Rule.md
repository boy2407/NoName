# Quản lý Enums

- **Vị trí**: `/Enums` thuộc `[[Domain Layer]]`.
- **Vai trò**: Lưu trữ dữ liệu cấu trúc cố định đại diện cho các trạng thái, loại, cấu hình hoặc hằng số. (Ví dụ: `TransactionStatus`, `OrderStatus`).
- **Quy tắc thiết kế**:
  - Không sử dụng Hardcode chuỗi String trong logic.
  - Phải ưu tiên được mapping dưới dạng String khi lưu xuống Database (`[[EF Core]]` qua Conversion) để dễ dàng đọc và truy xuất log.
  - (Mẫu: `modelBuilder.Entity<Transaction>().Property(e => e.Status).HasConversion<string>();`)