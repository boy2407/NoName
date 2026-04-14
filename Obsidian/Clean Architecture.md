# Kiến Trúc Hệ Thống (Clean Architecture)

Dự án này tuân theo một cách nghiêm ngặt mô hình **Clean Architecture** kết hợp hệ sinh thái .NET 8 và Blazor WebAssembly.

## Các tầng chính (Layers)
Bao gồm sự lệ thuộc theo hướng vào trong (Dependency Rule) từ ngoài vào lõi:
1. **Lõi Doanh Nghiệp**: [[Domain Layer]]
   - Nơi định nghĩa các Entities, Enums, Rule căn bản.
2. **Logic Ứng Dụng**: [[Application Layer]]
   - Dùng mô hình đặc trưng [[CQRS and MediatR]], quản lý Workflow, Mapping, Validation.
3. **Kết nối Hạ Tầng**: [[Infrastructure Layer]]
   - EF Core, Repositories, Redis, Services ngoài (Mail, AI).
4. **Chia Sẻ Chung (Shared)**: [[Shared Layer]]
   - Dự án `NoName.Shared`: Cầu nối DTO, Routes giữa Frontend và Backend.
5. **Giao Diện & API**: [[Presentation Layer]]
   - Bao gồm WebAPI (`NoName.BackendApi`) và ứng dụng Blazor: [[Blazor Wasm UI]].

## Tính năng kỹ thuật nổi bật
- **[CQRS + MediatR]([[CQRS and MediatR]])**: Tách biệt logic Read và Write rất rõ ràng thông qua Features. Mỏng Controllers.
- **[Primary Constructors]([[Infrastructure Layer]])**: Triệt để sử dụng C# 12 Primary Constructors cho việc Inject Dependencies thay vì hàm tạo truyền thống.
- **[Race Conditions]([[Concurrency Management]])**: Sử dụng khoá RedLock của Redis để xử lý chênh lệch Inventory (tồn kho).
- **[AI Agent]([[AI Plugins]])**: Có tính năng tích hợp AI Agent qua Semantic Kernel hoặc Ollama.