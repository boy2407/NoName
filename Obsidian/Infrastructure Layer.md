Tầng này chịu trách nhiệm thực thi các logic liên quan đến dữ liệu và dịch vụ bên ngoài.
Các thành phần chính và thư mục tương ứng:

1. **Database & ORM (`/EF`, `/Migrations`)**
   - Định nghĩa Database Context, cấu hình Fluent API: [[EF Core]]
   - Lịch sử thay đổi Database: [[Migrations]]

2. **Truy cập Dữ liệu (`/Persistence`)**
   - Quản lý Transaction: [[Unit of Work]]
   - Thực thi Repository Pattern: [[Persistence]]

3. **Dịch vụ Ngoài & Tiện ích (`/Services`)**
   - Các dịch vụ hệ thống như Mail, Storage, Caching, Payment: [[Services]]
   - Mail Service cụ thể: [[SendGrid]]
   - Quản lý đồng bộ/Xung đột (Redis RedLock): [[Concurrency Management]]

4. **Tích hợp Trí tuệ Nhân tạo (`/AIPlugins`)**
   - Các Service liên kết với Agent/LLM: [[AI Plugins]]

5. **Đăng ký Dependency Injection**
   - Nơi đăng ký vòng đời cho tất cả thành phần trên: [[Dependency Injection]]