# Quản lý Mapping Dữ liệu (DTO to Entity)

- **Vị trí**: `/Mapping` thuộc `[[Application Layer]]`.
- **Vai trò**: Nơi chuyển đổi giữa Request/Response DTO và cơ sở dữ liệu Entity. 
- **Quy tắc thiết kế**:
  - KHÔNG TRẢ về thẳng thực thể `[[Domain Layer]]` (Entity) qua API để bảo vệ cấu trúc CSDL thực tế. 
  - (Ví dụ: Chuyển đổi `ProductDto` từ API, mapping vào `Product` Entity và thêm xuống DB qua `[[Repository Pattern]]`).
  - Dùng các thư viện như `FastEndpoints`, `Mapster`, `AutoMapper` thông qua Interface (Profile map/Config map).